using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace ATSAudit
{
    public class UnauthorizedRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public UnauthorizedRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine(context.Response.StatusCode);

            if (context.User.Identity != null && !context.User.Identity.IsAuthenticated)
            {
                Console.WriteLine("Response failed!");
                // context.Response.Headers["Rafols"] = "Gyatt";
                // context.Response.Headers["Location"] = "https://localhost:7103/Login";
                context.Response.Redirect("https://localhost:7103/Login");
                return;
            }

            await _next(context);
        }
    }
}

