﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoExport.Models;

namespace MongoExport
{
    internal class Exporter
    {
        public void Run(ApplicationModel applicationModel)
        {
            const int exportBatchSize = 100;
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

            using(var writer = GetDestinationWriter(applicationModel))
            {
                stopwatch = Stopwatch.StartNew();
                var cursor = collection.Find(new QueryDocument()).SetBatchSize(exportBatchSize);
                writer.Write(cursor);
                stopwatch.Stop();
            }
        }

        private IDestinationWriter GetDestinationWriter(ApplicationModel applicationModel)
        {
            TextWriter textWriter;
            bool closeTextWriter;

            if (string.IsNullOrEmpty(applicationModel.File))
            {
                textWriter = Console.Out;
                closeTextWriter = false;
            }
            else
            {
                textWriter = new StreamWriter(applicationModel.File, false);
                closeTextWriter = true;
            }

            return new JsonDestinationWriter(textWriter, closeTextWriter);
        }
    }
}