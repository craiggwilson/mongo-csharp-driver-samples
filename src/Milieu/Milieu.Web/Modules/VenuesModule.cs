using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milieu.Web.Domain;
using Milieu.Web.Models;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;

namespace Milieu.Web.Modules
{
    public class VenuesModule : MilieuModule
    {
        private readonly MilieuDataContext _data;

        public VenuesModule(MilieuDataContext data)
            : base("/venues")
        {
            _data = data;

            this.RequiresAuthentication();

            Get[""] = _ =>
            {
                var model = new VenueListModel
                {
                    Venues = _data.Venues.FindAll().Select(x => new VenueListModel.Venue
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList()
                };

                return View["Index", model];
            };

            Get["/create"] = _ => View["Create"];

            Post["/create"] = _ =>
            {
                if(!Context.CurrentUser.HasClaim("Admin"))
                    return Response.AsRedirect("~/");

                var model = this.Bind<VenueCreateModel>();

                var venue = new Venue
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Name = model.Name,
                    Location = new Venue.VenueLocation { Geo = model.Geo }
                };

                _data.Venues.Insert(venue);
                return Response.AsRedirect("~/venues");
            };

            Get["/{id:guid}"] = p =>
            {
                Guid id = p.id;
                var venue = _data.Venues.AsQueryable()
                    .SingleOrDefault(x => x.Id == id.ToString("N"));

                if (venue == null)
                    return "No such venue exists.";

                var model = new VenueModel
                {
                    Id = venue.Id,
                    Name = venue.Name
                };

                return View["View", model];
            };

            // I Don't Like "Get" here...
            Get["/{id:guid}/checkin"] = p =>
            {
                Guid id = p.id;
                var venue = _data.Venues.AsQueryable()
                    .SingleOrDefault(x => x.Id == id.ToString("N"));

                if (venue == null)
                    return "No such venue exists.";

                var checkinTime = DateTime.UtcNow;

                // update user
                var args = new FindAndModifyArgs
                {
                    Query = Query<User>.Where(x => x.Email == Context.CurrentUser.UserName),
                    Update = Update.Combine(
                        Update<User>.Inc(x => x.TotalCheckins, 1),
                        Update.Inc("VenueCheckins." + venue.Id + ".Count", 1),
                        Update.Set("VenueCheckins." + venue.Id + ".Name", venue.Name),
                        Update.Set("VenueCheckins." + venue.Id + ".TimeUtc", checkinTime),
                        Update<User>.Set(x => x.LastCheckin.TimeUtc, checkinTime),
                        Update<User>.Set(x => x.LastCheckin.VenueId, venue.Id),
                        Update<User>.Set(x => x.LastCheckin.VenueName, venue.Name)),
                    VersionReturned = FindAndModifyDocumentVersion.Modified
                };

                var user = _data.Users.FindAndModify(args).GetModifiedDocumentAs<User>();

                // update venues
                bool firstTime = user.VenueCheckins[venue.Id].Count == 1;
                _data.Venues.Update(
                    Query<Venue>.Where(x => x.Id == venue.Id),
                    Update.Combine(
                        Update<Venue>.Inc(x => x.TotalCheckins, 1),
                        Update<Venue>.Inc(x => x.TotalUsers, firstTime ? 1 : 0)));

                // update checkins
                _data.Checkins.Update(
                    Query<Checkin>.Where(x => x.Id.UserId == user.Id && x.Id.VenueId == venue.Id),
                    Update.Combine(
                        Update<Venue>.SetOnInsert(x => x.Name, venue.Name),
                        Update<Checkin>.Push(x => x.TimesUtc, checkinTime)),
                    UpdateFlags.Upsert);

                // TODO: Deal with Mayor

                return Response.AsRedirect("~/venues/" + venue.Id);
            };
        }
    }
}