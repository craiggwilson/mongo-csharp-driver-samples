using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using NUnit.Framework;

namespace MongoExport
{
    [TestFixture]
    public class JsonDestinationWriterTests
    {
        [Test]
        public void Should_write_4_documents_on_seperate_lines()
        {
            var sb = new StringBuilder();
            var textWriter = new StringWriter(sb);
            var subject = new JsonDestinationWriter(textWriter, false);

            var documents = new List<BsonDocument>
            {
                new BsonDocument("x", 0).Add("y", 0),
                new BsonDocument("x", 0).Add("y", 1),
                new BsonDocument("x", 1).Add("y", 0),
                new BsonDocument("x", 1).Add("y", 1)
            };

            subject.Write(documents);

            var lines = sb.ToString().Split(new [] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            Assert.AreEqual(4, lines.Length);
        }
    }
}