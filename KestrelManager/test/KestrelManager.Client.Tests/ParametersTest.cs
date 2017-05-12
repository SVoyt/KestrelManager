using KestrelManager.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Kestrel.Client.Tests
{
    public class ParametersTest
    {
        [Fact]
        public void ConstructorTest()
        {
            var stringParameters = new[] { "-h","localhost","-a","yourapp","-remotepath","dist","-path","c:\\app" };
            var parameters = new Parameters(stringParameters);
            Assert.Equal(parameters.App, "yourapp");
            Assert.Equal(parameters.Host, "localhost");
            Assert.Equal(parameters.Path, "c:\\app");
            Assert.Equal(parameters.RemotePath, "dist");
        }
    }
}
