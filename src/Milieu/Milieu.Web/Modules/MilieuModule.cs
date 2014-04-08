using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Milieu.Web.Domain;
using MongoDB.Driver;
using Nancy;

namespace Milieu.Web.Modules
{
    public abstract class MilieuModule : NancyModule
    {
        protected MilieuModule()
        { }

        protected MilieuModule(string modulePath)
            : base(modulePath)
        { }
    }
}