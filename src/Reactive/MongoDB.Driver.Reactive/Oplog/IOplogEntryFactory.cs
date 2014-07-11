using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoDB.Driver.Reactive.Oplog
{
    public interface IOplogEntryFactory
    {
        OplogEntry Create(BsonDocument rawEntry);
    }
}