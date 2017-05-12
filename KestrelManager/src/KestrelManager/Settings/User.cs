using KestrelManager.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KestrelManager.Settings
{
    public class User
    {
        public string Name { get;set; }

        public string PrivateKey { get;set; }

        public IEnumerable<string> Apps {get;set;}
    }
}
