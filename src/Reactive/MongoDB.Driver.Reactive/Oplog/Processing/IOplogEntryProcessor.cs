using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Driver.Reactive.Oplog.Entries
{
    public interface IOplogEntryProcessor
    {
        void Process(OplogEntry entry);
    }

    public interface IOplogEntryProcessor<TEntry> where TEntry : OplogEntry
    {
        void Process(TEntry entry);
    }
}