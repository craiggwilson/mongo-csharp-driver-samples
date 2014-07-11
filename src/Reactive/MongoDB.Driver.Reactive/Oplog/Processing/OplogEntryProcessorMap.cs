using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Driver.Reactive.Oplog.Entries
{
    public class OplogEntryProcessorMap : IOplogEntryProcessor
    {
        private readonly Dictionary<Type, OplogEntryProcessorBase> _processors;

        public OplogEntryProcessorMap()
        {
            _processors = new Dictionary<Type, OplogEntryProcessorBase>();
        }

        public void Add<TEntry>(IOplogEntryProcessor<TEntry> processor) where TEntry : OplogEntry
        {
            _processors.Add(typeof(TEntry), new OplogEntryProcessor<TEntry>(processor));
        }

        public void Process(OplogEntry entry)
        {
            OplogEntryProcessorBase processorBase;
            if(!_processors.TryGetValue(entry.GetType(), out processorBase))
            {
                throw new NotSupportedException();
            }

            processorBase.Process(entry);
        }
    }
}