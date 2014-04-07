using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milieu.Web.Models;
using Nancy;

namespace Milieu.Web
{
    public class VenuesModule : NancyModule
    {
        public VenuesModule()
            : base("/venues")
        {
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