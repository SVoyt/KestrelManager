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
            StartCommand = options.StartCommand;
            StopCommand = options.StopCommand;
            StartArguments = options.StartArguments;
            StopArguments = options.StopArguments;
            AutoStart = options.AutoStart;
            State = State.Stopped;
        }

        public string StartCommand { get; }

        public string StopCommand { get; }

        public string StartArguments { get; }

        public string StopArguments { get; }

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
                var psi = new ProcessStartInfo(StartCommand);
                if ((!string.IsNullOrEmpty(Path)) && (Directory.Exists(Path)))
                    psi.WorkingDirectory = Path;
                if (_process==null)
                {
                    psi.Arguments = StartArguments;
                    psi.CreateNoWindow = true;
                    _process = Process.Start(psi);
                    _process.Exited += (sender, e) => {
                        State = State.Exited;
                        AdditionalInfo = "Exited after start";
                    }; 
                }
                else
                {
                    _process.Start();
                }
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
                if (!string.IsNullOrEmpty(StopCommand))
                {
                    var psi = new ProcessStartInfo(StopCommand);
                    if ((!string.IsNullOrEmpty(Path)) && (Directory.Exists(Path)))
                        psi.WorkingDirectory = Path;
                    psi.Arguments = StopArguments;
                    psi.CreateNoWindow = true;

                    using(var stopProcess = Process.Start(psi))
                    {
                        stopProcess.WaitForExit();
                    }
                }
             
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
