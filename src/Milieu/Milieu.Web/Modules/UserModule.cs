using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Milieu.Web.Domain;
using Milieu.Web.Models;
using MongoDB.Driver.Linq;
using Nancy;
using Nancy.Security;

namespace Milieu.Web.Modules
{
    public class UserModule : MilieuModule
    {
        private readonly MilieuDataContext _data;

        public UserModule(MilieuDataContext data)
            : base("/user")
        {
            _data = data;

            this.RequiresAuthentication();

            Get["/"] = _ => Response.AsRedirect("~/" + Context.CurrentUser.UserName);

            Get["/{email}"] = p =>
            {
                string email = p.email;
                if (!Context.CurrentUser.HasAnyClaim(new [] { "Admin" }) &&
                    (email != Context.CurrentUser.UserName || !Context.CurrentUser.HasClaim(Claims.UserDashboard)))
                    return "Unauthorized";

                var user = _data.Users.AsQueryable().SingleOrDefault(x => x.Email == email);

                var model = new UserDashboardModel
                {
                    Name = user.Name,
                    Email = user.Email,
                    TotalCheckins = user.TotalCheckins,
                    TotalLocations = user.VenueCheckins == null ? 0 : user.VenueCheckins.Count,
                    LatestCheckins = user.VenueCheckins
                        .OrderByDescending(x => x.Value.TimeUtc)
                        .Select(x => new UserDashboardModel.Checkin
                        {
                            VenueId = x.Key,
                            VenueName = x.Value.Name,
                            TimeUtc = x.Value.TimeUtc
                        })
                        .ToList()
                };

                return View["Dashboard", model];
            };
        }
    }
}