using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoDB.Driver.Reactive.Oplog
{
    public class NoOpOplogEntry : OplogEntry
    {
        public NoOpOplogEntry(BsonTimestamp timestamp, string @namespace, int version, string message)
            : base(timestamp, @namespace, version)
        {
            Message = message;
        }

        public string Message { get; private set; }

        public override OplogEntryType Type
        {
            get { return OplogEntryType.NoOp; }
        }
    }

    public class NoOpOplogEntryFactory : OplogEntryFactoryBase
    {
        public override OplogEntryType Type
        {
            get { return OplogEntryType.NoOp; }
        }

        protected override OplogEntry CreateCore(BsonDocument rawEntry)
        {
            return new NoOpOplogEntry(
                ExtractTimestamp(rawEntry),
                ExtractNamespace(rawEntry),
                ExtractVersion(rawEntry),
                (string)rawEntry["o"]["msg"]);
        }
    }
}