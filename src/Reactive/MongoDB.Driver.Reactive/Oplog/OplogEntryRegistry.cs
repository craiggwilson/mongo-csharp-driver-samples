using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace MongoDB.Driver.Reactive.Oplog
{
    public class OplogEntryRegistry : IOplogEntryFactory
    {
        public static OplogEntryRegistry CreateDefault()
        {
            var registry = new OplogEntryRegistry();
            registry.Register(new NoOpOplogEntryFactory());
            registry.Register(new DeleteOplogEntryFactory());
            registry.Register(new InsertOplogEntryFactory());
            registry.Register(new UpdateOplogEntryFactory());
            return registry;
        }

        private readonly List<IOplogEntryFactory> _factories;

        public OplogEntryRegistry()
        {
            _factories = new List<IOplogEntryFactory>();
        }

        public OplogEntry Create(BsonDocument rawEntry)
        {
            if (rawEntry == null) throw new ArgumentNullException("rawEntry");

            return _factories
                .Select(f => f.Create(rawEntry))
                .FirstOrDefault(e => e != null);
        }

        public void Register(IOplogEntryFactory factory)
        {
            if (factory == null) throw new ArgumentNullException("factory");

            _factories.Add(factory);
        }
    }
}