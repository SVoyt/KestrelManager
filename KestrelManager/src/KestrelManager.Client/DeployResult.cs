using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KestrelManager.Client
{
    public class DeployResult
    {
        public DeployResult(DeployState state)
        {
            State = state;
        }

        public DeployState State { get; }
    }
}
