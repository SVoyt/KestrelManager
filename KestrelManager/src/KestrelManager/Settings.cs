using KestrelManager.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KestrelManager
{
    public class Settings
    {
        public IEnumerable<AppOptions> ApplicationParameters { get; set; }
    }
}
