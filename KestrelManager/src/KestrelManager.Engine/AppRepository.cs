using System.Collections.Generic;
using KestrelManager.Engine.Apps;
using KestrelManager.Interfaces.Info;
using System.Linq;
using KestrelManager.Interfaces.App;
using KestrelManager.Interfaces;
using KestrelManager.Interfaces.Settings;

namespace KestrelManager.Engine
{
    public class AppRepository:IAppReporitory
    {
        private readonly IDictionary<int,IApp> _apps = new Dictionary<int,IApp>();

        public AppRepository(IEnumerable<AppOptions> appOptions)
        {
            var id = 1;
            foreach(var ao in appOptions)
            {
                var app = new App(ao);
                _apps.Add(id, app);
                if (app.AutoStart)
                    app.Start("Autostart");
                id++;
            }
        }

        public IList<AppInfo> GetApps()
        {
            return _apps.Select(c => new AppInfo(c.Key, c.Value)).ToList();
        }

        public bool StopApp(int id,string info="")
        {
            IApp app = null;
            if (!_apps.TryGetValue(id, out app))
                return false;
            return app.Stop(info);
        }

        public bool StartApp(int id, string info = "")
        {
            IApp app = null;
            if (!_apps.TryGetValue(id, out app) || (app.State== State.Started))
                return false;
            return app.Start(info);
        }

        public KeyValuePair<int,IApp> FindIdAppByName(string name)
        {
            return _apps.SingleOrDefault(c => c.Value.Name == name);
        }

        public void Dispose()
        {
            foreach(var app in _apps)
            {
                app.Value.Dispose();
            }
        }
    }
}
