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
                return;

            Task.Run(async () =>
            {
                using (var deployer = new Deployer(Directory.GetCurrentDirectory()))
                {
                    var result = await  deployer.Deploy(parameters);
                    Console.Write(result.State);
                }
            }).Wait();

            
        }
    }
}
