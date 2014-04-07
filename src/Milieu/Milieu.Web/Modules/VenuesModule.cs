using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milieu.Web.Models;
using Nancy;
using Nancy.Security;

namespace Milieu.Web.Modules
{
    public class VenuesModule : NancyModule
    {
        public VenuesModule()
            : base("/venues")
        {
            this.RequiresAuthentication();

            Get[""] = _ =>
            {
                var venue = new VenueModel
                {
                    Id = "Test",
                    Name = "MongoDB HQ"
                };
                return View["Index", venue];
            };
        }
    }
}