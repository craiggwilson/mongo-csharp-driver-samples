using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Driver.Reactive.Oplog.Entries
{
    internal class OplogEntryProcessor<TEntry> : OplogEntryProcessorBase
        where TEntry : OplogEntry
    {
        private readonly IOplogEntryProcessor<TEntry> _processor;

        public OplogEntryProcessor(IOplogEntryProcessor<TEntry> processor)
        {
            if (processor == null) throw new ArgumentNullException("processor");

            _processor = processor;
        }

        public override void Process(OplogEntry entry)
        {
            _processor.Process((TEntry)entry);
        }
    }
}