using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoImport.Models
{
    public class ApplicationModel
    {
        public string Hostname { get; set; }

        public int Port { get; set; }

        public string DatabaseName { get; set; }

        public string CollectionName { get; set; }

        public string File { get; set; }

        public string Type { get; set; }
    }
}