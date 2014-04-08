using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milieu.Web.Domain;
using Milieu.Web.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.ModelBinding;
using Nancy.Validation;

namespace Milieu.Web.Modules
{
    public class DefaultModule : MilieuModule
    {
        private readonly MilieuDataContext _dataContext;

        public DefaultModule(MilieuDataContext dataContext)
        {
            _dataContext = dataContext;

            Get[""] = _ =>
            {
                return View["Index"];
            };

            Get["/login"] = p =>
            {
                return View["Login"];
            };

            Post["/login"] = p =>
            {
                var model = this.Bind<RegisterModel>();

                var user = _dataContext.Users.AsQueryable()
                    .SingleOrDefault(x => x.Email == model.Email);

                // THESE ARE NOT THE PLAINTEXT PASSWORDS YOU ARE LOOKING FOR!
                if(user == null || user.Password != model.Password)
                {
                    return "Username or password are invalid.";
                }

                return this.Login(Guid.Parse(user.Id), fallbackRedirectUrl: "~/user/" + user.Email);
            };

            Get["/logout"] = _ =>
            {
                return this.LogoutAndRedirect("~/");
            };

            Post["/register"] = p =>
            {
                var model = this.Bind<RegisterModel>();

                var user = new User
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password,
                    Claims = new[] { Claims.VenueCheckin, Claims.UserDashboard },
                    VenueCheckins = new Dictionary<string, User.VenueCheckin>()
                };

                try
                {
                    _dataContext.Users.Save(user);
                }
                catch(MongoDuplicateKeyException)
                {
                    return "Someone already has your username.";
                }

                return this.Response.AsRedirect(string.Format("~/user/{0}", user.Email));
            };
        }
    }
}