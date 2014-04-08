using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Milieu.Web.Models
{
    public class VenueCreateModel
    {
        public string Name { get; set; }

        public long[] Geo { get; set; }
    }
}