using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milieu.Web.Domain;
using Milieu.Web.Utilities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Nancy.ViewEngines.Razor;

namespace Milieu.Web
{
    public class MilieuBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            ConfigureMongoDBDriver();

            var db = new MongoClient()
                .GetServer()
                .GetDatabase("milieu");

            var data = new MilieuDataContext(db);
            container.Register(new MilieuDataContext(db));

            ConfigureMongoDBServer(data);
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            container.Register<IUserMapper, MilieuUserMapper>();
        }

        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            var formsAuthConfig = new FormsAuthenticationConfiguration
            {
                RedirectUrl = "~/login",
                UserMapper = container.Resolve<IUserMapper>()
            };
            FormsAuthentication.Enable(this.ApplicationPipelines, formsAuthConfig);
        }

        private void ConfigureMongoDBDriver()
        {
            MongoDefaults.GuidRepresentation = GuidRepresentation.Standard;

            var pack = new ConventionPack();
            pack.Add(new CamelCaseElementNameConvention());
            pack.Add(new IgnoreIfNullConvention(true));

            ConventionRegistry.Register("Milieu Conventions", pack, t => true);

            BsonClassMap.RegisterClassMap<User>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(x => x.VenueCheckins)
                    .SetSerializer(new GuidDictionaryDocumentSerializer<User.VenueCheckin>());
            });
        }

        private void ConfigureMongoDBServer(MilieuDataContext data)
        {
            // Create Indexs
            data.Venues.CreateIndex(IndexKeys<Venue>.GeoSpatial(x => x.Location.Geo));

            if (data.Users.Count() == 0)
            {
                var adminUser = new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Craig Wilson",
                    Email = "craiggwilson@gmail.com",
                    Password = "password",
                    Claims = new List<string> { Claims.Admin, Claims.UserDashboard, Claims.VenueCheckin }
                };

                data.Users.Save(adminUser);
            }

            if(data.Venues.Count() == 0)
            {
                var venue1 = new Venue
                {
                    Id = Guid.NewGuid(),
                    Name = "Panera Bread",
                    Location = new Venue.VenueLocation
                    {
                        Geo = new[] { -96.772827, 32.872667 }
                    }
                };

                var venue2 = new Venue
                {
                    Id = Guid.NewGuid(),
                    Name = "Maggianos",
                    Location = new Venue.VenueLocation
                    {
                        Geo = new[] { -96.773062, 32.867026 }
                    }
                };

                var venue3 = new Venue
                {
                    Id = Guid.NewGuid(),
                    Name = "Blue Mesa Grill",
                    Location = new Venue.VenueLocation
                    {
                        Geo = new[] { -96.773491, 32.864341 }
                    }
                };

                var venue4 = new Venue
                {
                    Id = Guid.NewGuid(),
                    Name = "Gordon Biersch",
                    Location = new Venue.VenueLocation
                    {
                        Geo = new[] { -96.768685, 32.870018 }
                    }
                };

                data.Venues.InsertBatch(new [] { venue1, venue2, venue3, venue4 });
            }
        }
    }
}