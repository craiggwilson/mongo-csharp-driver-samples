﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Milieu.Web.Models
{
    public class VenueListModel
    {
        public List<Venue> Venues { get; set; }

        public class Venue
        {
            public string Id { get; set; }

            public string Name { get; set; }
        }
    }
}