using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace Milieu.Web.Domain
{
    public class User
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public IEnumerable<string> Claims { get; set; }

        public int TotalCheckins { get; set; }

        public LastVenueCheckin LastCheckin { get; set; }

        [BsonDictionaryOptions(DictionaryRepresentation.Document)]
        public Dictionary<string, VenueCheckin> VenueCheckins { get; set; }

        public class LastVenueCheckin
        {
            public string VenueId { get; set; }

            public string VenueName { get; set; }

            public DateTime TimeUtc { get; set; }
        }

        public class VenueCheckin
        {
            public string Name { get; set; }

            public int Count { get; set; }

            public DateTime TimeUtc { get; set; }
        }
    }
}