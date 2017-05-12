using KestrelManager.Interfaces.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KestrelManager.Settings
{
    public class Security
    {
        public IEnumerable<User> Users {get;set;}
    }
}
