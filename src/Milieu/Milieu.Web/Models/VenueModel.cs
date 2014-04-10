using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milieu.Web.Models
{
    public class VenueModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int TotalCheckins { get; set; }

        public int TotalUsers { get; set; }

        public double[] Geo { get; set; }

        public List<NearbyVenue> Nearby { get; set; }

        public class NearbyVenue
        {
            public Guid Id { get; set; }

            public string Name { get; set; }

            public double[] Geo { get; set; }
        }
    }
}