using KestrelManager.Engine.Extensions;
using System;
using System.IO;
using System.IO.Compression;

namespace KestrelManager.Engine
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

        public bool Deploy(byte[] fileContent, string path)
        {
            var guid = Guid.NewGuid();
            var deployPath = Path.Combine(_deployerPath, guid.ToString());
            //exception
            Directory.CreateDirectory(deployPath);
            var deployPacket = Path.Combine(deployPath, packetFile);
            using (var fs = new FileStream(deployPacket,FileMode.Create))
            {
                fs.Write(fileContent,0,fileContent.Length);
            }
            //exception
            using (var za = ZipFile.Open(deployPacket, ZipArchiveMode.Read))
            {
                za.ExtractToDirectory(path, true);
            }
            Directory.Delete(deployPath, true);
            //exception
            return true;
        }

        public void Dispose()
        {
            if (Directory.Exists(_deployerPath))
            {
                Directory.Delete(_deployerPath, true);
            }
        }

    }
}
