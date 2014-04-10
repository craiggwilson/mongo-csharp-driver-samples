﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Milieu.Web.Domain
{
    public class Venue
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int TotalCheckins { get; set; }

        public int TotalUsers { get; set; }

        public VenueLocation Location { get; set; }

        public class VenueLocation
        {
            public double[] Geo { get; set; }
        }
    }
}