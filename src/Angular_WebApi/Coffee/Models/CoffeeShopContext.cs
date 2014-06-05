using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Driver.Builders;

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

            _db.GetCollection("shops").CreateIndex(
                IndexKeys<Shop>.GeoSpatial(x => x.Location));
        }

        public MongoCollection<Order> Orders
        {
            get { return _db.GetCollection<Order>("orders"); }
        }

        public MongoCollection<Shop> Shops
        {
            get { return _db.GetCollection<Shop>("shops"); }
        }
    }
}