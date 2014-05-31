using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ImportData
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = new MongoClient().GetServer().GetDatabase("coffee");
            var shops = db.GetCollection<BsonDocument>("shops");

            var xml = XDocument.Load("Oslo.xml");

            var docs = xml.Root
                .Elements("node")
                .Select(n => new BsonDocument
                {
                    { "name", n.Elements().Single(x => x.Attribute("k").Value == "name").Attribute("v").Value },
                    { "loc", new BsonArray(new [] { double.Parse(n.Attribute("lon").Value), double.Parse(n.Attribute("lat").Value) } ) }
                });

            shops.InsertBatch(docs);
        }
    }
}