namespace KestrelManager.Interfaces.Settings
{
    public class AppOptions
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string RunCommand { get; set; }

        public bool AutoStart { get; set; }

        public string Password { get; set; }
    }
}
