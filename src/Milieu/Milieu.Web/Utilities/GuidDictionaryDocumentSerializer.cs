using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Milieu.Web.Utilities
{
    public class GuidDictionaryDocumentSerializer<TValue> : BsonBaseSerializer, IBsonDocumentSerializer
    {
        public override object Deserialize(BsonReader bsonReader, Type nominalType, Type actualType, IBsonSerializationOptions options)
        {
            var bsonType = bsonReader.GetCurrentBsonType();
            if (bsonType == BsonType.Null)
            {
                bsonReader.ReadNull();
                return null;
            }
            else if (bsonType == BsonType.Document)
            {
                var dictionary = new Dictionary<Guid, TValue>();

                var valueDiscriminatorConvention = BsonSerializer.LookupDiscriminatorConvention(typeof(TValue));
                bsonReader.ReadStartDocument();
                while (bsonReader.ReadBsonType() != BsonType.EndOfDocument)
                {
                    var key = Guid.Parse(bsonReader.ReadName());
                    var valueType = valueDiscriminatorConvention.GetActualType(bsonReader, typeof(TValue));
                    var valueSerializer = BsonSerializer.LookupSerializer(valueType);
                    var value = (TValue)valueSerializer.Deserialize(bsonReader, typeof(TValue), valueType, null);
                    dictionary.Add(key, value);
                }
                bsonReader.ReadEndDocument();

                return dictionary;
            }

            var message = string.Format("Can't deserialize a {0} from BsonType {1}.", nominalType.FullName, bsonType);
            throw new BsonSerializationException(message);
        }

        public override void Serialize(BsonWriter bsonWriter, Type nominalType, object value, IBsonSerializationOptions options)
        {
            if (value == null)
            {
                bsonWriter.WriteNull();
                return;
            }

            var document = (IEnumerable<KeyValuePair<Guid, TValue>>)value;
            bsonWriter.WriteStartDocument();
            foreach (var pair in document)
            {
                bsonWriter.WriteName(pair.Key.ToString());
                BsonSerializer.Serialize<TValue>(bsonWriter, pair.Value);
            }
            bsonWriter.WriteEndDocument();
        }

        public BsonSerializationInfo GetMemberSerializationInfo(string memberName)
        {
            return new BsonSerializationInfo(
                memberName,
                BsonSerializer.LookupSerializer(typeof(TValue)),
                typeof(TValue),
                null);
        }
    }
}