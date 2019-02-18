using System.Web.Http;
using WebActivatorEx;
using WSAManager.Web;
using Swashbuckle.Application;
using System;

//[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace WSAManager.Web
{
    public class SwaggerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            config.EnableSwagger(x => x.SingleApiVersion("v1", "API Documentation")).EnableSwaggerUi();
        }
    }
}
