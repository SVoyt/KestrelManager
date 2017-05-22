using System;

namespace KestrelManager.Interfaces.App
{
    public interface IApp:IDisposable
    {
        string StartCommand { get; }

        string StartArguments { get; }

		string StopCommand { get; }

		string StopArguments { get; }

        string Name { get; }

        string Path { get; }

        bool AutoStart { get; }

        State State { get;  }

        bool Start(string info = "");

        bool Stop(string info = "");

        string AdditionalInfo{get;}
    }
}
