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

        public Statistics Stats { get; set; }

        public class Statistics
        {
            public int TotalCheckins { get; set; }

            public int TotalLocations { get; set; }

            public string LastVenueName { get; set; }

            public DateTime? LastCheckinTimeUtc { get; set; }
        }
    }
}