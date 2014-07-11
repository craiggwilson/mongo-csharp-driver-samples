using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Driver.Reactive.Oplog.Entries
{
    internal abstract class OplogEntryProcessorBase : IOplogEntryProcessor
    {
        public abstract void Process(OplogEntry entry);
    }
}