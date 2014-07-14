using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Driver.Reactive.Oplog.Processing
{
    public class OplogProcessor
    {
        private readonly MongoClient _client;
        private readonly IOplogEntryProcessor _entryProcessor;

        public OplogProcessor(MongoClient client, IOplogEntryProcessor entryProcessor)
        {
            if (client == null) throw new ArgumentNullException("client");
            if (entryProcessor == null) throw new ArgumentNullException("entryProcessor");

            _client = client;
            _entryProcessor = entryProcessor;
        }

        public void Start()
        {
            _client.TailOplog()
                .Subscribe(
                    onNext: OnEntry);
        }

        public void Stop()
        {

        }

        private void OnEntry(OplogEntry entry)
        {
            _entryProcessor.Process(entry);
        }
    }
}