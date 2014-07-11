using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDB.Driver.Reactive
{
    public static class TailableExtensions
    {
        public static IObservable<T> Tail<T>(this MongoCollection<T> collection, IMongoQuery initialQuery = null, Func<T, IMongoQuery> queryFactory = null, IMongoFields fields = null)
        {
            return new MongoTailableCursorObservable<T>(collection, initialQuery, queryFactory, fields);
        }
    }
}