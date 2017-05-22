using System;
using System.Collections.Generic;
using KestrelManager.Interfaces.Info;

namespace KestrelManager.Client
{
    public class ActionResult
    {
        public ActionResult(ActionState state, string message = "", IEnumerable<AppInfo> apps = null)
        {
            State = state;
            Message = message;
            Apps = apps;
        }

        public ActionState State { get; }

        public string Message { get; }

        public IEnumerable<AppInfo> Apps { get; }
    }
}
