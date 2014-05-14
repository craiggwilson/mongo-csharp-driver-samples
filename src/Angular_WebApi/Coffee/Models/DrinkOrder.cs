using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Coffee.Models
{
    public class DrinkOrder
    {
        public DrinkType Type { get; set; }

        public string Size { get; set; }

        public string Drinker { get; set; }
    }
}