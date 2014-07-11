using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoDB.Driver.Reactive.Oplog
{
    public class UpdateOplogEntry : OplogEntry
    {
        public UpdateOplogEntry(BsonTimestamp timestamp, string @namespace, int version, BsonValue id, BsonDocument updates)
            : base(timestamp, @namespace, version)
        {
            Id = id;
            Updates = updates;
        }

        public BsonValue Id { get; private set; }

        public BsonDocument Updates { get; private set; }

        public override OplogEntryType Type
        {
            get { return OplogEntryType.Update; }
        }
    }

    public class UpdateOplogEntryFactory : OplogEntryFactoryBase
    {
        public override OplogEntryType Type
        {
            get { return OplogEntryType.Update; }
        }

        protected override OplogEntry CreateCore(BsonDocument rawEntry)
        {
            return new UpdateOplogEntry(
                ExtractTimestamp(rawEntry),
                ExtractNamespace(rawEntry),
                ExtractVersion(rawEntry),
                rawEntry["o2"]["_id"],
                (BsonDocument)rawEntry["o"]);
        }
    }
}