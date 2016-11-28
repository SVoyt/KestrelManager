using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KestrelManager.Client
{
    public class Parameters
    {
        public string Host { get; }

        public string App { get; }

        public string RemotePath { get; }

        public string Path { get; }

        public Parameters(string[] args)
        {
            for (var i = 0; i < args.Length; i++)
            {
                switch (args[i])
                {
                    case "-h":
                        Host = args[i + 1];
                        break;
                    case "-a":
                        App = args[i + 1];
                        break;
                    case "-remotepath":
                        RemotePath = args[i + 1];
                        break;
                    case "-path":
                        Path = args[i + 1];
                        break;
                }
            }
        }

        public bool IsFilled()
        {   
            //too much resources
            return (new[] { Host, App, Path }).All(c => !string.IsNullOrEmpty(c));
        }
    }
}
