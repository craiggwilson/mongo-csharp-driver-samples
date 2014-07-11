using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoDB.Driver.Reactive.Oplog
{
    public class DeleteOplogEntry : OplogEntry
    {
        public DeleteOplogEntry(BsonTimestamp timestamp, string @namespace, int version, BsonValue id)
            : base(timestamp, @namespace, version)
        {
            Id = id;
        }

        public BsonValue Id { get; private set; }

        public override OplogEntryType Type
        {
            get { return OplogEntryType.Delete; }
        }
    }

    public class DeleteOplogEntryFactory : OplogEntryFactoryBase
    {
        public override OplogEntryType Type
        {
            get { return OplogEntryType.Delete; }
        }

        protected override OplogEntry CreateCore(BsonDocument rawEntry)
        {
            return new DeleteOplogEntry(
                ExtractTimestamp(rawEntry),
                ExtractNamespace(rawEntry),
                ExtractVersion(rawEntry),
                rawEntry["o"]["_id"]);
        }
    }
}