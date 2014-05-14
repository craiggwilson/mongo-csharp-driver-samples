using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fclp;
using MongoExport.Models;

namespace MongoExport
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var result = Parse(args);

            if (result.Item1.HasErrors)
            {
                Console.WriteLine(result.Item1.ErrorText);
                return;
            }

            var exporter = new Exporter();
            exporter.Run(result.Item2);
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
                .As('o', "out")
                .WithDescription("Specifies a file to write the export to. If you do not specify a file name, the mongoexport writes data to standard output (e.g. stdout).");

            return Tuple.Create(builder.Parse(args), builder.Object);
        }
    }
}