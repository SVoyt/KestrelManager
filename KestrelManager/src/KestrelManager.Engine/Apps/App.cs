using KestrelManager.Interfaces.App;
using KestrelManager.Interfaces.Settings;
using System;
using System.Diagnostics;
using System.IO;

namespace KestrelManager.Engine.Apps
{
    public class App: IApp
    {
        public App(AppOptions options)
        {
            Path = options.Path;
            Name = options.Name;
            RunCommand = options.RunCommand;
            AutoStart = options.AutoStart;
            State = State.Stopped;
        }

        public string RunCommand { get; }

        public string Name { get; }

        public string Path { get;  }

        public bool AutoStart { get;  }

        public State State { get; private set; }

        public string AdditionalInfo {get; private set;}

        private Process _process;

        public bool Start(string info="")
        {
            try
            {
                var psi = new ProcessStartInfo(RunCommand);
                if ((!string.IsNullOrEmpty(Path)) && (Directory.Exists(Path)))
                    psi.WorkingDirectory = Path;
                if (_process==null)
                {
                    _process = Process.Start(psi);
                }
                else
                {
                    _process.Start();
                }
                
                //check if exit
                State = State.Started;
                AdditionalInfo = info;
                return true;
            }
            catch(Exception e)
            {
                State = State.Error;
                AdditionalInfo = e.Message;
                return false;
            }
        }

        public bool Stop(string info="")
        {
            try
            {
                if (_process == null)
                    return false;
                _process.Kill();
                State = State.Stopped;
                AdditionalInfo = info;
                return true;              
            }
            catch (Exception e)
            {
                State  = State.Error;
                AdditionalInfo = e.Message;
                return false;
            }
        }

        public void Dispose()
        {
            if (_process!=null)
                _process.Dispose();
        }
    }
}
