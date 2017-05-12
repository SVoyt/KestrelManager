using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using KestrelManager.Middlewares;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace KestrelManager.Tests
{
    public class RSASecurityMiddlewareTest
    {
        [Fact]
        public void TestInvoke()
        {
            const string PRIVATE_KEY = @"-----BEGIN RSA PRIVATE KEY-----
MIICXAIBAAKBgQCqGKukO1De7zhZj6+H0qtjTkVxwTCpvKe4eCZ0FPqri0cb2JZfXJ/DgYSF6vUp
wmJG8wVQZKjeGcjDOL5UlsuusFncCzWBQ7RKNUSesmQRMSGkVb1/3j+skZ6UtW+5u09lHNsj6tQ5
1s1SPrCBkedbNf0Tp0GbMJDyR4e9T04ZZwIDAQABAoGAFijko56+qGyN8M0RVyaRAXz++xTqHBLh
3tx4VgMtrQ+WEgCjhoTwo23KMBAuJGSYnRmoBZM3lMfTKevIkAidPExvYCdm5dYq3XToLkkLv5L2
pIIVOFMDG+KESnAFV7l2c+cnzRMW0+b6f8mR1CJzZuxVLL6Q02fvLi55/mbSYxECQQDeAw6fiIQX
GukBI4eMZZt4nscy2o12KyYner3VpoeE+Np2q+Z3pvAMd/aNzQ/W9WaI+NRfcxUJrmfPwIGm63il
AkEAxCL5HQb2bQr4ByorcMWm/hEP2MZzROV73yF41hPsRC9m66KrheO9HPTJuo3/9s5p+sqGxOlF
L0NDt4SkosjgGwJAFklyR1uZ/wPJjj611cdBcztlPdqoxssQGnh85BzCj/u3WqBpE2vjvyyvyI5k
X6zk7S0ljKtt2jny2+00VsBerQJBAJGC1Mg5Oydo5NwD6BiROrPxGo2bpTbu/fhrT8ebHkTz2epl
U9VQQSQzY1oZMVX8i1m5WUTLPz2yLJIBQVdXqhMCQBGoiuSoSjafUhV7i1cEGpb88h5NBYZzWXGZ
37sJ5QsW+sJyoNde3xH8vdXhzU7eT82D6X/scw9RZz+/6rCJ4p0=
-----END RSA PRIVATE KEY-----";

            const string ENCRYPTED_AUTH = @"lpTls5gZEErc2GqdfIyln+tIddZPtL7BwBYDgKpg0bzggO/LgWwZy3JRdPHqrznGEC/GXpCuuM9ZwefjvjtciyOqnBOPhaRdlv6o0WBY5PDPiQT2h0FvP9SVhc9kKqOpZbA6i3ZnIFALRIzpIA33G8+6FN6Oor3WEQDHlMiX4z0=";

            var middleware = new RSASecurityMiddleware( (httpcontext) => { return new Task(() => { }); } , new StringReader(PRIVATE_KEY));
            var context = new DefaultHttpContext();
            context.Request.Headers.Add("Authorization", new StringValues(ENCRYPTED_AUTH));
            middleware.Invoke(context);
            Assert.NotNull(context.User);
        }
    }
}
