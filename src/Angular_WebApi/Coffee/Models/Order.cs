using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Coffee.Models
{
    public class Order
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CoffeeShopId { get; set; }

        public DrinkType Type { get; set; }

        public string Size { get; set; }

        public string Drinker { get; set; }

        public string[] SelectedOptions { get; set; }
    }
}