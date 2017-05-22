using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KestrelManager.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var parameters = new Parameters(args);
            if (!parameters.IsFilled())
                // show help
                return;

            Task.Run(async () =>
            {
                var keyContent = string.Empty;
                if (!string.IsNullOrEmpty(parameters.KeyPath) && File.Exists(parameters.KeyPath))
                {
                    try
                    {
                        keyContent = File.ReadAllText(parameters.KeyPath);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
                var manager = new Manager(parameters.Host, parameters.User, keyContent);
                ActionResult result = null;
                switch(parameters.Command)
                {
                    case Command.List:
                        result = await manager.List();
                        break;
                    case Command.Start:
                        result = await manager.Start(parameters.AppId);
                        break;
                    case Command.Stop:
                        result = await manager.Stop(parameters.AppId);
                        break;
                    case Command.Deploy:
                        result = await manager.Deploy(parameters.AppId, parameters.Path, parameters.RemotePath);
                        break;
                }
                if (result!=null)
                {
                    Console.WriteLine("{0} | {1}", result.State, result.Message);
                    if (result.Apps!=null)
                    {
                        foreach(var app in result.Apps)
                        {
                            Console.WriteLine("{0} {1} {2} {3}", app.Id, app.Name, app.State, app.Additional);
                        }
                    }
                }

            }).Wait();

            
        }
    }
}
