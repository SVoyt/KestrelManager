using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KestrelManager.Client
{
    public class Parameters
    {
        public Command Command { get; } = Command.List;

        public string User { get; set; }

        public string KeyPath { get; set; }
        
        public string Host { get; }

        public int AppId { get; }

        public string RemotePath { get; }

        public string Path { get; }

        public Parameters(string[] args)
        {
            for (var i = 0; i < args.Length; i++)
            {
                var name = args[i];
                var value = args[i + 1];
                switch (name)
                {
                    case "-c":
                        Command = (Command)Enum.Parse(typeof(Command), value, true);
                        break;
                    case "-u":
                        User = value;
                        break;
                    case "-k":
                        KeyPath = value;
                        break;
                    case "-h":
                        Host = value;
                        break;
                    case "-a":
                        var appId = 0;
                        int.TryParse(value, out appId);
                        AppId = appId;
                        break;
                    case "-rp":
                        RemotePath = value;
                        break;
                    case "-p":
                        Path = value;
                        break;
                }
            }
        }

        public bool IsFilled()
        {
            return true;
        }
    }
}
