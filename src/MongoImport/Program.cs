using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fclp;
using MongoImport.Models;

namespace MongoImport
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var result = Parse(args);

            if(result.Item1.HasErrors)
            {
                Console.WriteLine(result.Item1.ErrorText);
                return;
            }

            var importer = new Importer();
            importer.Run(result.Item2);
        }

        private static Tuple<ICommandLineParserResult, ApplicationModel> Parse(string[] args)
        {
            var builder = new FluentCommandLineBuilder<ApplicationModel>();

            builder.Setup(x => x.Hostname)
                .As('h', "host")
                .SetDefault("localhost")
                .WithDescription("The hostname of the database.");

            builder.Setup(x => x.Port)
                .As("port")
                .SetDefault(27017)
                .WithDescription("The port of the database.");

            builder.Setup(x => x.DatabaseName)
                .As('d', "db")
                .Required()
                .WithDescription("database");

            builder.Setup(x => x.CollectionName)
                .As('c', "collection")
                .Required()
                .WithDescription("collection");

            builder.Setup(x => x.File)
                .As("file")
                .Required()
                .WithDescription("The file from which to read the data.");

            builder.Setup(X => X.Type)
                .As("type")
                .SetDefault("json")
                .WithDescription("Declare the type of export format to import.");

            return Tuple.Create(builder.Parse(args), builder.Object);
        }
    }
}