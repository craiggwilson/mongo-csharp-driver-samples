using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoDB.Driver.Reactive.Oplog
{
    public class UnrecognizedOplogEntryException : Exception
    {
        public UnrecognizedOplogEntryException(string type)
            : base(string.Format("Unrecognized oplog entry {{op: {0}}}.", type))
        {
        }
    }
}
