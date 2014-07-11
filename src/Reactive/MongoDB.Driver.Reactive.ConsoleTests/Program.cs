using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace MongoDB.Driver.Reactive.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new MongoClient();
            var collection = client.GetServer()
                .GetDatabase("test")
                .GetCollection<BsonDocument>("foo");

            collection.Drop();
            collection.Database.CreateCollection("foo", CollectionOptions.SetCapped(true).SetMaxSize(100).SetMaxDocuments(10));

            for(int i = 0; i < 20; i++)
            {
                collection.Insert(new BsonDocument("_id", i));
            }


            var observable = collection.Tail();
                //queryFactory: doc => Query.GTE("_id", doc["_id"]));

            var cancellationSource = new CancellationTokenSource();

            observable.Subscribe(
                onNext: doc => Console.WriteLine(doc),
                onError: ex => Console.WriteLine(ex.Message),
                onCompleted: () => Console.WriteLine("Finished!!!"),
                token: cancellationSource.Token);

            Console.WriteLine("Press any key to cancel!");
            Console.ReadKey();
            cancellationSource.Cancel();
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}