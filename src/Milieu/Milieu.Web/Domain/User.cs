using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Milieu.Web.Utilities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace Milieu.Web.Domain
{
    public class User
    {
        public User()
        {
            Claims = new List<string>();
            LastCheckin = new LastVenueCheckin();
            VenueCheckins = new Dictionary<Guid, VenueCheckin>();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public List<string> Claims { get; set; }

        public int TotalCheckins { get; set; }

        public LastVenueCheckin LastCheckin { get; set; }

        public Dictionary<Guid, VenueCheckin> VenueCheckins { get; private set; }

        public class LastVenueCheckin
        {
            public Guid? VenueId { get; set; }

            public string VenueName { get; set; }

            public DateTime? TimeUtc { get; set; }
        }

        public class VenueCheckin
        {
            public string Name { get; set; }

            public int Count { get; set; }

            public DateTime TimeUtc { get; set; }
        }
    }
}