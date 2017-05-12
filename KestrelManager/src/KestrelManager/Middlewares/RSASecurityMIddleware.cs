using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Security.Claims;
using System;
using System.Collections.Generic;
using System.Text;
using KestrelManager.Engine.Crypto;

namespace KestrelManager.Middlewares
{
    public class RSASecurityMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly RsaDecryptor _decryptor;

        public RSASecurityMiddleware(RequestDelegate next, TextReader keyReader)
        {
            _next = next;
            _decryptor = new RsaDecryptor(keyReader);
        }

        public Task Invoke(HttpContext context)
        {
            var authorization = new StringValues();
            context.Request.Headers.TryGetValue("Authorization", out authorization);
            
            if (authorization.Count>0){
                var firstAuthValue = authorization.First();
                var username = _decryptor.Decrypt(firstAuthValue);
                if (!string.IsNullOrEmpty(username)){
                    var identity = new ClaimsIdentity(new []{ new Claim("username", username) });
                    context.User = new ClaimsPrincipal(identity);
                }
            }

            return this._next(context);
        }

    }
}