using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Coffee.Models
{
    public class CoffeeShopContext
    {
        private static readonly MongoDatabase _db = new MongoClient()
            .GetServer()
            .GetDatabase("coffee");

        static CoffeeShopContext()
        {
            var conventions = new ConventionPack();
            conventions.Add(new CamelCaseElementNameConvention());
            ConventionRegistry.Register("coffeeshop", conventions, t => true);
        }

        public MongoCollection<Order> Orders
        {
            get { return _db.GetCollection<Order>("orders"); }
        }
    }
}