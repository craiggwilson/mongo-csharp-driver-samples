using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Reactive.Oplog;
using MongoDB.Driver.Reactive.Oplog.Entries;

namespace MongoDB.Driver.Reactive.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new MongoClient();

            var cancellationSource = new CancellationTokenSource();

            client.TailOplog()
                .Subscribe(
                    onNext: entry => Console.WriteLine(entry.Namespace + " " + entry.Type),
                    onError: ex => Console.WriteLine(ex.Message),
                    onCompleted: () => Console.WriteLine("Finished!!!"),
                    token: cancellationSource.Token);

            Console.WriteLine("Press any key to cancel!");
            Console.ReadKey();
            cancellationSource.Cancel();
            Console.WriteLine("Press any key to exit");
        }
    }
}