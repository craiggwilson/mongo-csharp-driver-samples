using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoDB.Driver.Reactive.Oplog
{
    public class InsertOplogEntry : OplogEntry
    {
        public InsertOplogEntry(BsonTimestamp timestamp, string @namespace, int version, BsonDocument document)
            : base(timestamp, @namespace, version)
        {
            Document = document;
        }

        public BsonDocument Document { get; private set; }

        public override OplogEntryType Type
        {
            get { return OplogEntryType.Insert; }
        }
    }

    public class InsertOplogEntryFactory : OplogEntryFactoryBase
    {
        public override OplogEntryType Type
        {
            get { return OplogEntryType.Insert; }
        }

        protected override OplogEntry CreateCore(BsonDocument rawEntry)
        {
            return new InsertOplogEntry(
                ExtractTimestamp(rawEntry),
                ExtractNamespace(rawEntry),
                ExtractVersion(rawEntry),
                (BsonDocument)rawEntry["o"]);
        }
    }
}