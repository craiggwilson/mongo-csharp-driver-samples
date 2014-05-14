using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Coffee.Controllers
{
    public class HomeController : ApiController
    {
        public HttpResponseMessage Get()
        {
            var response = new HttpResponseMessage();
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Content", "index.html");
            response.Content = new StreamContent(File.Open(file, FileMode.Open));
            return response;
        }
    }
}