using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milieu.Web.Domain;
using Milieu.Web.Models;
using MongoDB.Bson;
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
        private readonly MilieuDataContext _data;

        public DefaultModule(MilieuDataContext data)
        {
            _data = data;

            Get["/"] = _ =>
            {
                var userAvg = GetAveragePipeline("userId");
                var venueAvg = GetAveragePipeline("venueId");

                var model = new IndexModel
                {
                    AverageCheckinsPerUser = (int)Math.Floor(_data.Checkins.Aggregate(userAvg).ResultDocuments.Single()["avg"].ToDouble()),
                    AverageCheckinsPerVenue = (int)Math.Floor(_data.Checkins.Aggregate(venueAvg).ResultDocuments.Single()["avg"].ToDouble())
                };

                return View["Index", model];
            };

            Get["/login"] = p =>
            {
                return View["Login"];
            };

            Post["/login"] = p =>
            {
                var model = this.Bind<RegisterModel>();

                var user = _data.Users.AsQueryable()
                    .SingleOrDefault(x => x.Email == model.Email);

                // THESE ARE NOT THE PLAINTEXT PASSWORDS YOU ARE LOOKING FOR!
                if(user == null || user.Password != model.Password)
                {
                    return "Username or password are invalid.";
                }

                return this.Login(user.Id, fallbackRedirectUrl: "~/user/" + user.Email);
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
                    Id = Guid.NewGuid(),
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password, // YES, I KNOW!!!
                    Claims = new List<string> { Claims.VenueCheckin, Claims.UserDashboard },
                };

                try
                {
                    _data.Users.Save(user);
                }
                catch(MongoDuplicateKeyException)
                {
                    return "Someone already has your username.";
                }

                return this.Response.AsRedirect(string.Format("~/user/{0}", user.Email));
            };
        }

        private IEnumerable<BsonDocument> GetAveragePipeline(string groupFieldName)
        {
            var firstGroup = new BsonDocument("$group", new BsonDocument
            {
                { "_id", "$_id." + groupFieldName },
                { "sum", new BsonDocument
                        {
                            { "$sum", new BsonDocument("$size", "$timesUtc") }
                        }}
            });

            var secondGroup = new BsonDocument("$group", new BsonDocument
            {
                { "_id", 1 },
                { "avg", new BsonDocument("$avg", "$sum") }
            });

            yield return firstGroup;
            yield return secondGroup;
        }
    }
}