using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using NUnit.Framework;

namespace MongoImport.Tests
{
    public class JsonSourceParserTests
    {
        private JsonSourceParser _subject;

        [SetUp]
        public void Before()
        {
            _subject = new JsonSourceParser();
        }

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

            List<RawBsonDocument> documents;
            using(var ms = new MemoryStream())
            using (var writer = new StreamWriter(ms))
            {
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
                writer.Flush();

                ms.Position = 0;

                documents = _subject.ReadDocuments(ms).ToList();
            }

            Assert.AreEqual(4, documents.Count);
        }
    }
}