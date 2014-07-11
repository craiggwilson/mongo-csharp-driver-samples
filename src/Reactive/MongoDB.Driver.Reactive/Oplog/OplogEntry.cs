using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoDB.Driver.Reactive.Oplog
{
    public abstract class OplogEntry
    {
        protected OplogEntry(BsonTimestamp timestamp, string @namespace, int version)
        {
            Timestamp = timestamp;
            Namespace = @namespace;
            Version = version;
        }

        public BsonTimestamp Timestamp { get; private set; }

        public string Namespace { get; private set; }

        public int Version { get; private set; }

        public abstract OplogEntryType Type { get; }
    }
}