using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Reactive.Oplog.Entries;

namespace MongoDB.Driver.Reactive.Oplog
{
    public static class OplogExtensions
    {
        public static IObservable<OplogEntry> TailOplog(this MongoClient client, IOplogEntryFactory entryFactory = null, BsonTimestamp initialTimestamp = null, bool throwOnUnknownEntryTypes = false)
        {
            if(client == null) throw new ArgumentNullException("client");

            var collection = client
                .GetServer()
                .GetDatabase("local")
                .GetCollection<BsonDocument>("oplog.rs");

            if(entryFactory == null)
            {
                entryFactory = OplogEntryRegistry.CreateDefault();
            }

            if(initialTimestamp == null)
            {
                initialTimestamp = new BsonTimestamp(0, 0);
            }

            return collection.Tail(
                initialQuery: Query.GTE("ts", initialTimestamp),
                queryFactory: doc => Query.GT("ts", doc["ts"]))
                .Select(rawEntry =>
                {
                    var entry = entryFactory.Create(rawEntry);
                    if (entry == null && throwOnUnknownEntryTypes)
                    {
                        throw new UnrecognizedOplogEntryException((string)rawEntry["op"]);
                    }

                    return entry;
                })
                .Where(entry => entry != null);
        }
    }
}