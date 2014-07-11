using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoDB.Driver.Reactive.Oplog
{
    public abstract class OplogEntryFactoryBase : IOplogEntryFactory
    {
        public abstract OplogEntryType Type { get; }

        public OplogEntry Create(BsonDocument rawEntry)
        {
            var entryType = rawEntry["op"].AsString;
            if(entryType != Type.Type)
            {
                return null;
            }

            return CreateCore(rawEntry);
        }

        protected abstract OplogEntry CreateCore(BsonDocument rawEntry);

        protected string ExtractNamespace(BsonDocument rawEntry)
        {
            return (string)rawEntry["ns"];
        }

        protected BsonTimestamp ExtractTimestamp(BsonDocument rawEntry)
        {
            return (BsonTimestamp)rawEntry["ts"];
        }

        protected int ExtractVersion(BsonDocument rawEntry)
        {
            return rawEntry["v"].ToInt32();
        }
    }
}