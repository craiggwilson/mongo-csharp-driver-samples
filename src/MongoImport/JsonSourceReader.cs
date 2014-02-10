using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace MongoImport
{
    internal class JsonSourceReader : ISourceReader
    {
        private readonly static IBsonSerializer _serializer;
        private readonly bool _closeReader;
        private readonly TextReader _reader;

        static JsonSourceReader()
        {
            _serializer = BsonDocumentSerializer.Instance;
        }

        public JsonSourceReader(TextReader reader, bool closeReader)
        {
            _reader = reader;
            _closeReader = closeReader;
        }

        public void Dispose()
        {
            if(_closeReader)
            {
                _reader.Close();
            }
        }

        public IEnumerable<BsonDocument> ReadDocuments()
        {
            // 1 document per line
            string line;
            while ((line = _reader.ReadLine()) != null)
            {
                if(string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                using (var bsonReader = JsonReader.Create(line))
                {
                    yield return (BsonDocument)_serializer.Deserialize(bsonReader, typeof(BsonDocument), null);
                }
            }
        }
    }
}