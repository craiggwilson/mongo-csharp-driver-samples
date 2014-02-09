using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoImport
{
    internal interface ISourceParser
    {
        IEnumerable<RawBsonDocument> ReadDocuments(Stream stream);
    }
}