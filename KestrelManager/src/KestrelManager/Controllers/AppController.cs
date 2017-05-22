using KestrelManager.Engine;
using KestrelManager.Engine.Apps;
using KestrelManager.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KestrelManager.Controllers
{
    [Route("api/v1/[controller]")]
    public class AppController : Controller
    {
        readonly IAppReporitory _appRepository;

        public AppController(IAppReporitory appRepository)
        {
            _appRepository = appRepository;
        }

        [HttpGet]
        public IActionResult Apps()
        {
            return Ok(_appRepository.GetApps());
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public IActionResult Start(int id)
        {
            return Ok(_appRepository.StartApp(id));
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public IActionResult Stop(int id)
        {
            return Ok(_appRepository.StopApp(id));
        }

        [HttpPost]
        [Route("[action]/{id}")]
        public IActionResult Deploy(int id)
        {
            var deployData = JsonConvert.DeserializeObject<DeployData>(Request.Form["data"][0]);
            var app = _appRepository.FindIdAppByName(deployData.App);

            var path = app.Value.Path;
            if (!string.IsNullOrEmpty(deployData.RemotePath))
            {
                path = Path.Combine(path, deployData.RemotePath);
            }
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            _appRepository.StopApp(app.Key, "Stopped for deploy");

            using (var deployer = new Deployer(Directory.GetCurrentDirectory()))
            {
                deployer.Deploy(ReadFileBytesFromRequest(), path);
            }

            _appRepository.StartApp(app.Key, "Started after deploy");
            return Ok();
        }

        private byte[] ReadFileBytesFromRequest()
        {
            var file = Request.Form.Files[0];
            var fileBytes = new byte[file.Length];
            using (var stream = file.OpenReadStream())
            {
                stream.Read(fileBytes, 0, (int)stream.Length);
            }
            return fileBytes;
        }
    }
}
