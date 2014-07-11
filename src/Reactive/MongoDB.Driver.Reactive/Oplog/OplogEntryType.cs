using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoDB.Driver.Reactive.Oplog
{
    public sealed class OplogEntryType : IEquatable<OplogEntryType>
    {
        public static OplogEntryType Delete = new OplogEntryType("d");
        public static OplogEntryType Insert = new OplogEntryType("i");
        public static OplogEntryType NoOp = new OplogEntryType("n");
        public static OplogEntryType Update = new OplogEntryType("u");

        private readonly string _type;

        public OplogEntryType(string type)
        {
            if(type == null) throw new ArgumentNullException("type");

            _type = type;
        }

        public string Type
        {
            get { return _type; }
        }

        public bool Equals(OplogEntryType other)
        {
            if(other == null)
            {
                return false;
            }

            return _type == other._type;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as OplogEntryType);
        }

        public override int GetHashCode()
        {
            return _type.GetHashCode();
        }

        public override string ToString()
        {
            return _type;
        }

        public static bool operator==(OplogEntryType lhs, OplogEntryType rhs)
        {
            if(lhs == null)
            {
                return false;
            }

            return lhs.Equals(rhs);
        }

        public static bool operator!=(OplogEntryType lhs, OplogEntryType rhs)
        {
            return !(lhs == rhs);
        }
    }
}