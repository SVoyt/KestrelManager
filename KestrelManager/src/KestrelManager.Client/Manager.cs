using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using KestrelManager.Engine.Crypto;
using KestrelManager.Interfaces.Info;
using Newtonsoft.Json;

namespace KestrelManager.Client
{
    /// <summary>
    /// Manager for client operations.
    /// </summary>
    public class Manager
    {
        const string PACKET_FILE = "packet.zip";


        /// <summary>
        /// Gets the host name with port where is kestrelmanager
        /// </summary>
        /// <value>The host.</value>
        public string Host { get; }

        /// <summary>
        /// Gets the user name.
        /// </summary>
        /// <value>The user.</value>
        public string User { get; }

        /// <summary>
        /// Gets the public key content. Required if username specified.
        /// </summary>
        /// <value>The public key.</value>
        public string Key { get; }

        public string DeployerDirectory { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:KestrelManager.Client.Manager"/> class.
        /// </summary>
        /// <param name="host">Host (with port).</param>
        /// <param name="user">User.</param>
        /// <param name="key">Key.</param>
        public Manager(string host, string user, string key, string deployerDirectory = null)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException(nameof(host));
            }
            Host = host;
            User = user;
            if (!string.IsNullOrEmpty(user) & string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key), "Key should be specified with user");
            }
            Key = key;
            DeployerDirectory = string.IsNullOrEmpty(deployerDirectory) ? Directory.GetCurrentDirectory() : deployerDirectory;
        }


        public async Task<ActionResult> List()
        {
            return await HttpAction(async (client) =>
             {
                var responseMessage = await client.GetAsync(UriForRequest(Command.List));
                 return await CreateResultByResponse(responseMessage);
             });
        }

        public async Task<ActionResult> Start(int appId)
        {
            return await HttpAction(async (client) =>
            {
                var responseMessage = await client.GetAsync(UriForRequest(Command.Start, appId));
                return await CreateResultByResponse(responseMessage);
            });
        }

        public async Task<ActionResult> Stop(int appId)
        {
            return await HttpAction(async (client) =>
            {
                var responseMessage = await client.GetAsync(UriForRequest(Command.Stop, appId));
                return await CreateResultByResponse(responseMessage);
            });
        }

        public async Task<ActionResult> Deploy(int appId, string path, string remotePath)
        {
            try
            {
                CheckDeployerDir();
                var deployWorkspace = Path.Combine(DeployerDirectory, Guid.NewGuid().ToString());
                Directory.CreateDirectory(deployWorkspace);
                var packetPath = Path.Combine(deployWorkspace, PACKET_FILE);
                ZipFile.CreateFromDirectory(path, packetPath);

                return await HttpAction(async (client) =>
                {
                    var requestContent = new MultipartFormDataContent();
                    using (var stream = new FileStream(packetPath, FileMode.Open))
                    {
                        var content = new StreamContent(stream);
                        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = "packet",
                            FileName = PACKET_FILE
                        };
                        requestContent.Add(content);
                        var data = new { AppId = appId, RemotePath = remotePath };
                        var jsonData = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");
                        requestContent.Add(jsonData, "data");

                        var responseMessage = await client.PostAsync(UriForRequest(Command.Deploy, appId), requestContent);
                        return await CreateResultByResponse(responseMessage);
                    }
                });
            }
            catch (Exception e)
            {
                return new ActionResult(ActionState.Error, e.Message);
            }
        }

        private async Task<ActionResult> HttpAction(Func<HttpClient, Task<ActionResult>> action)
        {
            using (var client = CreateHttpClient())
            {
                try
                {
                    return await action(client);
                }
                catch (Exception e)
                {
                    return new ActionResult(ActionState.Error, e.Message);
                }
            }
        }

        private async Task<ActionResult> CreateResultByResponse(HttpResponseMessage responseMessage)
        {
            switch (responseMessage.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    return new ActionResult(ActionState.Ok, "Ok", JsonConvert.DeserializeObject<IEnumerable<AppInfo>>(await responseMessage.Content.ReadAsStringAsync()));
                case System.Net.HttpStatusCode.Unauthorized:
                    return new ActionResult(ActionState.Unauthorized, await responseMessage.Content.ReadAsStringAsync());
                case System.Net.HttpStatusCode.Forbidden:
                    return new ActionResult(ActionState.Forbidden, await responseMessage.Content.ReadAsStringAsync());
                default:
                    return new ActionResult(ActionState.Error, await responseMessage.Content.ReadAsStringAsync());

            }
        }

        private Uri UriForRequest(Command command, int appId = 0)
        {
            if (command == Command.List)
            {
                return new Uri(new Uri(Host), "api/v1/apps");
            }
            else
            {
                return new Uri(new Uri(Host), string.Format("api/v1/apps/{0}/{1}", command.ToString().ToLower(), appId));
            }
        }

        private HttpClient CreateHttpClient()
        {
            var client = new HttpClient();
            if (!string.IsNullOrEmpty(User))
            {
                client.DefaultRequestHeaders.Add("Authorization", CreateAuthSignature());
            }
            return client;
        }

        private void CheckDeployerDir()
        {
            if (!Directory.Exists(DeployerDirectory))
			{
				Directory.CreateDirectory(DeployerDirectory);
			}
        }

        /// <summary>
        /// Creates the authentication signature for request.
        /// </summary>
        /// <returns>The auth signature.</returns>
        private string CreateAuthSignature()
        {
            var encryptor = new RsaEncryptor(new StringReader(Key));

			var epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
			var timeSpan = DateTime.UtcNow - epochStart;
			var requestTimeStamp = Convert.ToUInt64(timeSpan.TotalSeconds).ToString();

            return string.Format(
                "{0} {1}",
                User,
        		encryptor.Encrypt(string.Format("{0}_{1}", requestTimeStamp,Guid.NewGuid().ToString("N")))
            );
        }

    }
}
