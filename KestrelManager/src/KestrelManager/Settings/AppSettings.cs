using KestrelManager.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KestrelManager.Settings
{
    public class AppSettings
    {
        public IEnumerable<AppOptions> ApplicationParameters { get; set; }

        public Security Security { get;set;}
    }
}
