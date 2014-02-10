using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoExport
{
    internal interface IDestinationWriter : IDisposable
    {
        void Write(IEnumerable<BsonDocument> documents);
    }
}