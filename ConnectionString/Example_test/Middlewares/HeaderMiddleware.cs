using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Example_test.Middlewares
{
    public class HeaderMiddleware
    {

        private readonly RequestDelegate _next;

        public HeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task Invoke(HttpContext context)
        {
            var watch = new Stopwatch();
            watch.Start();

            //To add Headers AFTER everything you need to do this
            context.Response.OnStarting(state => {
                var httpContext = (HttpContext)state;
                httpContext.Response.Headers.Add("s17159", new[] { watch.ElapsedMilliseconds.ToString() });

                return Task.CompletedTask;
            }, context);

            await _next(context);
        }
    }
}
