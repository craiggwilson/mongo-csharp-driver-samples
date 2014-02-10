using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using NUnit.Framework;

namespace MongoExport
{
    [TestFixture]
    public class ProgramTests
    {
        [Test]
        public void Should_export_json_to_file()
        {
            var client = new MongoClient();
            var db = client.GetServer().GetDatabase("mongoexport");
            db.Drop();

            var collection = db.GetCollection("small");
            collection.Insert(new BsonDocument("x", 0).Add("y", 0));
            collection.Insert(new BsonDocument("x", 0).Add("y", 1));
            collection.Insert(new BsonDocument("x", 1).Add("y", 0));
            collection.Insert(new BsonDocument("x", 1).Add("y", 1));

            var args = new[] 
            {
                "-h", "localhost",
                "--port", "27017",
                "-d", "mongoexport",
                "-c", "small",
                "--out", "small.json"
            };

            Program.Main(args);

            int numLines = 0;
            using(var reader = File.OpenText("small.json"))
            {
                string line;
                while((line = reader.ReadLine()) != null)
                {
                    if(string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    numLines++;
                }
            }

            Assert.AreEqual(4, numLines);
        }
    }
}
