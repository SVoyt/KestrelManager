using KestrelManager.Interfaces.App;


namespace KestrelManager.Interfaces.Info
{
    public class AppInfo
    {
        public AppInfo(int id,IApp app)
        {
            Id = id;
            Name = app.Name; 
            State = app.State.ToString();
            Additional  = app.AdditionalInfo;
        }

        public int Id { get; }

        public string Name { get; }

        public string State { get; }

        public string Additional { get; }
    }
}
