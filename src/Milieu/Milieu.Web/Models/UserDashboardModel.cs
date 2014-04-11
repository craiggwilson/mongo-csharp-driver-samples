using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Milieu.Web.Models
{
    public class UserDashboardModel
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public int TotalCheckins { get; set; }
        
        public int TotalLocations { get; set; }

        public List<Checkin> LatestCheckins { get; set; }

        public class Checkin
        {
            public Guid VenueId { get; set; }

            public string VenueName { get; set; }

            public DateTime TimeUtc { get; set; }
        }
    }
}