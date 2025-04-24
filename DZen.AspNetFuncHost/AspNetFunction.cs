using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace DZen.AspNetFuncHost
{
    public class AspNetFunction
    {
        private readonly IServiceProvider provider;
        private readonly RequestDelegate reqDel;
        private readonly ILogger<AspNetFunction> logger;

        public AspNetFunction(IServiceProvider provider, RequestDelegate reqDel, ILogger<AspNetFunction> logger)
        {
            this.provider = provider;
            this.reqDel = reqDel;
            this.logger = logger;
        }

        [Function(nameof(AspNetFuncHost))]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", "patch", "put", Route = "{*route}")] HttpRequest req)
        {
            DefaultHttpContext context = new DefaultHttpContext();
            context.RequestServices = provider;
            context.Request.Host = req.Host;
            context.Request.Path = req.Path;
            context.Request.Method = req.Method;
            context.Request.Scheme = req.Scheme;
            context.Request.Protocol = req.Protocol;

            foreach (var header in req.Headers)
            {
                context.Request.Headers.Append(header.Key, header.Value);
            }

            context.Response.Body = new MemoryStream();//replace with object pool. not sure if MemoryStream can be replaced

            await reqDel(context);
            return new PreExecutedResult(context);
        }
    }
}
