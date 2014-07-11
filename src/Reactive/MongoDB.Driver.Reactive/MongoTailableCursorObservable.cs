using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDB.Driver.Reactive
{
    internal class MongoTailableCursorObservable<T> : ObservableBase<T>
    {
        private readonly object _lock = new object();
        private readonly MongoCollection<T> _collection;
        private readonly IMongoFields _fields;
        private readonly IMongoQuery _initialQuery;
        private readonly List<IObserver<T>> _observers;
        private readonly Func<T, IMongoQuery> _queryFactory;
        private readonly Thread _thread;

        public MongoTailableCursorObservable(MongoCollection<T> collection, IMongoQuery initialQuery = null, Func<T, IMongoQuery> queryFactory = null, IMongoFields fields = null)
        {
            if (collection == null) throw new ArgumentNullException("collection");

            _collection = collection;
            _initialQuery = initialQuery;
            _queryFactory = queryFactory;
            _fields = fields;

            _observers = new List<IObserver<T>>();
            _thread = new Thread(Run)
            {
                IsBackground = true,
                Name = "Tailable Cursor on " + collection.FullName
            };
        }

        public bool HasObservers
        {
            get
            {
                lock(_lock)
                {
                    return _observers.Count > 0;
                }
            }
        }

        protected override IDisposable SubscribeCore(IObserver<T> observer)
        {
            lock (_lock)
            {
                if (!_observers.Contains(observer))
                {
                    _observers.Add(observer);
                }

                if (_thread.ThreadState.HasFlag(ThreadState.Unstarted))
                {
                    _thread.Start();
                }
            }

            return Disposable.Create(() => Unsubscribe(observer));
        }

        private MongoCursor<T> CreateCursor(IMongoQuery query)
        {
            MongoCursor<T> cursor;
            if (query != null)
            {
                cursor = _collection.Find(query).SetFields(_fields);
            }
            else
            {
                cursor = _collection.FindAll().SetFields(_fields);
            }
            var flags = QueryFlags.TailableCursor;
            if (_collection.Database.Name == "local" && _collection.Name == "oplog.rs")
            {
                flags |= (QueryFlags)8;
            }
            return cursor.SetFlags(flags);
        }

        private void Notify(Action<IObserver<T>> notification)
        {
            IObserver<T>[] copy;
            lock (_lock)
            {
                copy = _observers.ToArray();
            }
            foreach (var observer in copy)
            {
                notification(observer);
            }
        }

        private void Run()
        {
            IMongoQuery query = _initialQuery;
            try
            {
                while (HasObservers)
                {
                    foreach (var doc in CreateCursor(query))
                    {
                        Notify(o => o.OnNext(doc));
                        if(_queryFactory != null)
                        {
                            // Might not need to do this on every 
                            // iteration. However, it can provide
                            // a progress report for external
                            // storage.
                            query = _queryFactory(doc);
                        }

                        if (!HasObservers)
                        {
                            break;
                        }
                    }

                    if (query == null)
                    {
                        Notify(o => o.OnCompleted());
                        break;
                    }

                    Thread.Sleep(100);
                }
            }
            catch(Exception ex)
            {
                Notify(o => o.OnError(ex));
            }
        }

        private void Unsubscribe(IObserver<T> observer)
        {
            lock (_lock)
            {
                _observers.Remove(observer);
                if(_observers.Count == 0)
                {
                    _thread.Join(TimeSpan.FromMilliseconds(10));
                }
            }
        }
    }
}