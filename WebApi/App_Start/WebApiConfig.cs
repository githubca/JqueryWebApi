using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using WebApiContrib.Formatting.Jsonp;
using System.Web.Http.Cors;

namespace WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Formatters.JsonFormatter.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            //var jsonpFormatter = new JsonpMediaTypeFormatter(config.Formatters.JsonFormatter);
            ////config.Formatters.Add(jsonpFormatter);
            //config.Formatters.Insert(0, jsonpFormatter);
            //EnableCorsAttribute cors = new EnableCorsAttribute("*", "*", "*");
            //config.EnableCors(cors);
            config.EnableCors();
        }
    }
}
