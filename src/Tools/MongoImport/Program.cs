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
                .WithDescription("Specifies a resolvable hostname for the mongod from which you want to export data.");

            builder.Setup(x => x.Port)
                .As("port")
                .SetDefault(27017)
                .WithDescription("Specifies the port number, if the MongoDB instance is not running on the standard port. (i.e. 27017)");

            builder.Setup(x => x.DatabaseName)
                .As('d', "db")
                .Required()
                .WithDescription("Specifies the name of the database that contains the collection you want to export.");

            builder.Setup(x => x.CollectionName)
                .As('c', "collection")
                .Required()
                .WithDescription("Specifies the collection that you want mongoexport to export.");

            builder.Setup(x => x.File)
                .As("file")
                .Required()
                .WithDescription("Specifies a file to read the data from. If you do not specify a file name, the mongoimport reads data from standard input (e.g. stdin).");

            builder.Setup(X => X.Type)
                .As("type")
                .SetDefault("json")
                .WithDescription("Declare the type of export format to import.");

            return Tuple.Create(builder.Parse(args), builder.Object);
        }
    }
}