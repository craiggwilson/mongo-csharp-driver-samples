using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Serializers;

namespace MongoImport
{
    internal class JsonSourceParser : ISourceParser
    {
        public IEnumerable<RawBsonDocument> ReadDocuments(Stream stream)
        {
            var serializer = new RawBsonDocumentSerializer();

            // 1 document per line
            using (var streamReader = new StreamReader(stream))
            {
                string line;
                while((line = streamReader.ReadLine()) != null)
                {
                    using (var bsonReader = JsonReader.Create(line))
                    {
                        yield return (RawBsonDocument)serializer.Deserialize(bsonReader, typeof(RawBsonDocument), null);
                    }
                }
            }
        }
    }
}