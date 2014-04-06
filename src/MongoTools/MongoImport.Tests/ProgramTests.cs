using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using NUnit.Framework;

namespace MongoImport
{
    public class ProgramTests
    {
        [Test]
        public void Should_import_json_to_database()
        {
            var client = new MongoClient();
            var db = client.GetServer().GetDatabase("mongoimport");
            db.Drop();

            var args = new[] 
            {
                "-h", "localhost",
                "--port", "27017",
                "-d", "mongoimport",
                "-c", "small",
                "--file", "small.json"
            };

            Program.Main(args);

            var col = db.GetCollection("small");

            Assert.AreEqual(4, col.Count());
        }
    }
}