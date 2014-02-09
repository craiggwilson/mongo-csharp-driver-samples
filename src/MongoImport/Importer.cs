using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoImport.Models;

namespace MongoImport
{
    internal class Importer
    {
        public void Run(ApplicationModel applicationModel)
        {
            const int importBatchSize = 100;
            int totalCount = 0;
            Stopwatch stopwatch;
            var source = GetSourceParser(applicationModel.Type);

            var settings = new MongoClientSettings
            {
                ConnectionMode = ConnectionMode.Direct,
                Server = new MongoServerAddress(applicationModel.Hostname, applicationModel.Port),
            };

            var client = new MongoClient(settings);
            var database = client.GetServer().GetDatabase(applicationModel.DatabaseName);
            var collection = database.GetCollection<RawBsonDocument>(applicationModel.CollectionName);

            using (var stream = new FileStream(applicationModel.File, FileMode.Open))
            {
                stopwatch = Stopwatch.StartNew();
                List<RawBsonDocument> documents = new List<RawBsonDocument>();
                foreach(var document in source.ReadDocuments(stream))
                {
                    documents.Add(document);
                    totalCount++;
                    if(documents.Count == importBatchSize)
                    {
                        InsertBatch(collection, documents);
                        documents.Clear();
                        Console.WriteLine("{0} - Inserted {1}", stopwatch.Elapsed, totalCount);
                    }
                }

                if(documents.Count > 0)
                {
                    InsertBatch(collection, documents);
                    documents.Clear();
                    Console.WriteLine("{0} - Inserted {1}", stopwatch.Elapsed, totalCount);
                }
                stopwatch.Stop();
            }

            Console.WriteLine("Finished in {0}.", stopwatch.Elapsed);
        }

        private ISourceParser GetSourceParser(string type)
        {
            if(type.Equals("json", StringComparison.InvariantCultureIgnoreCase))
            {
                return new JsonSourceParser();
            }

            throw new NotSupportedException(string.Format("{0} is not a supported type.", type));
        }

        private void InsertBatch(MongoCollection<RawBsonDocument> collection, List<RawBsonDocument> documents)
        {
            collection.InsertBatch(documents);
        }
    }
}