using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Middleware
{
    public class IPFilterMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string[] _bannedIp = { "192.168.0.1" };

        public IPFilterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {

            if (_bannedIp.Any(i => i == context.Connection.RemoteIpAddress.ToString()))
            {
                context.Response.StatusCode = 401;
            }
            else
            {
                await _next(context);

            }

        }
    }
}
