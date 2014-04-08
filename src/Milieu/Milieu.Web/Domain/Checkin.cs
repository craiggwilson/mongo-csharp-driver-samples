using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Milieu.Web.Domain
{
    public class Checkin
    {
        public CheckinId Id { get; set; }

        public string VenueName { get; set; }

        public List<DateTime> TimesUtc { get; set; }

        public class CheckinId
        {
            public string UserId { get; set; }

            public string VenueId { get; set; }
        }
    }
}