using KestrelManager.Interfaces.App;
using KestrelManager.Interfaces.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KestrelManager.Interfaces
{
    public interface IAppReporitory:IDisposable
    {
        IList<AppInfo> GetApps();

        bool StopApp(int id, string info = "");

        bool StartApp(int id, string info = "");

        KeyValuePair<int, IApp> FindIdAppByName(string name);
    }
}
