using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milieu.Web.Models;
using Nancy;

namespace Milieu.Web
{
    public class HomeModule : NancyModule
    {
        public HomeModule()
        {
            Get[""] = _ =>
            {
                return View["Index"];
            };
        }
    }
}