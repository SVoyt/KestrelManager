using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace KestrelManager.Client
{
    public class Deployer:IDisposable
    {
        readonly string _deployerPath;
        const string packetFile = "packet.zip";

        public Deployer(string path)
        {
            _deployerPath = Path.Combine(path, "deployer");
            if (!Directory.Exists(_deployerPath))
            {
                Directory.CreateDirectory(_deployerPath);
            }
        }

        public async Task<DeployResult> Deploy(Parameters par)
        {
            var deployWorkspace = Path.Combine(_deployerPath, Guid.NewGuid().ToString());
            Directory.CreateDirectory(deployWorkspace);
            var packetPath = Path.Combine(deployWorkspace, packetFile);
            ZipFile.CreateFromDirectory(par.Path, packetPath);

            using (var client = new HttpClient())
            {
                var requestContent = new MultipartFormDataContent();
                using (var stream = new FileStream(packetPath, FileMode.Open))
                {
                    var content = new StreamContent(stream);
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "packet",
                        FileName = packetFile
                    };
                    requestContent.Add(content);
                    var data = new { App = par.App, RemotePath = par.RemotePath };
                    var jsonData = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
                    requestContent.Add(jsonData, "data");

                    var response = await client.PostAsync(par.Host + "/api/v1/app/deploy", requestContent);
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                        return new DeployResult(DeployState.Unauthorized);
                    else
                        return new DeployResult(DeployState.Ok);
                }

            }


        }

        public void Dispose()
        {
            if (Directory.Exists(_deployerPath))
            {
                Directory.Delete(_deployerPath,true);
            }
        }


    }
}
