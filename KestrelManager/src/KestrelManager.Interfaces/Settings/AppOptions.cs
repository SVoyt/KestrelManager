namespace KestrelManager.Interfaces.Settings
{
    public class AppOptions
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string StartCommand { get; set; }

        public string StopCommand { get; set; }

        public string StartArguments { get; set; }

        public string StopArguments { get; set; }

        public bool AutoStart { get; set; }

        public string Password { get; set; }
    }
}
