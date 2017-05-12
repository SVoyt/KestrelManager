using System.IO;
using Microsoft.AspNetCore.Builder;

namespace KestrelManager.Middlewares
{
    public static class MiddlewareExtensions
    {
		public static IApplicationBuilder UseRSASecurity(
			this IApplicationBuilder builder, TextReader keyReader)
		{
            return builder.UseMiddleware<RSASecurityMiddleware>(keyReader);
		}
    }
}
