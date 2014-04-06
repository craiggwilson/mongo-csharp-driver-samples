using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using NUnit.Framework;

namespace MongoImport
{
    [TestFixture]
    public class JsonSourceReaderTests
    {
        [Test]
        public void Should_parse_4_documents_on_different_lines()
        {
            var lines = new List<string>
            {
                "{ x: 0, y: 0}",
                "{ x: 0, y: 1}",
                "{ x: 1, y: 0}",
                "{ x: 1, y: 1}"
            };

            List<BsonDocument> documents;
            using (var reader = GetReader(lines))
            {
                documents = reader.ReadDocuments().ToList();
            }

            Assert.AreEqual(4, documents.Count);
        }

        private JsonSourceReader GetReader(IEnumerable<string> lines)
        {
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            foreach (var line in lines)
            {
                writer.WriteLine(line);
            }
            writer.Flush();

            ms.Position = 0;

            return new JsonSourceReader(new StreamReader(ms), true);
        }
    }
}