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

            var settings = new MongoClientSettings
            {
                ConnectionMode = ConnectionMode.Direct,
                Server = new MongoServerAddress(applicationModel.Hostname, applicationModel.Port),
            };

            var client = new MongoClient(settings);
            var database = client.GetServer().GetDatabase(applicationModel.DatabaseName);
            var collection = database.GetCollection<BsonDocument>(applicationModel.CollectionName);

            using (var reader = GetSourceReader(applicationModel))
            {
                stopwatch = Stopwatch.StartNew();
                var documents = new List<BsonDocument>();
                foreach (var document in reader.ReadDocuments())
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

        private ISourceReader GetSourceReader(ApplicationModel applicationModel)
        {
            TextReader textReader;
            bool closeTextReader;
            if(string.IsNullOrEmpty(applicationModel.File))
            {
                textReader = Console.In;
                closeTextReader = false;
            }
            else
            {
                textReader = new StreamReader(applicationModel.File);
                closeTextReader = true;
            }

            if(applicationModel.Type.Equals("json", StringComparison.InvariantCultureIgnoreCase))
            {
                return new JsonSourceReader(textReader, closeTextReader);
            }

            throw new NotSupportedException(string.Format("{0} is not a supported type.", applicationModel.Type));
        }

        private void InsertBatch(MongoCollection<BsonDocument> collection, List<BsonDocument> documents)
        {
            collection.InsertBatch(documents);
        }
    }
}