using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;

namespace Milieu.Web.Domain
{
    public class MilieuDataContext
    {
        private readonly MongoDatabase _db;

        public MilieuDataContext(MongoDatabase db)
        {
            _db = db;
        }

        public MongoCollection<Checkin> Checkins
        {
            get { return _db.GetCollection<Checkin>("checkins"); }
        }

        public MongoCollection<User> Users
        {
            get { return _db.GetCollection<User>("users"); }
        }

        public MongoCollection<Venue> Venues
        {
            get { return _db.GetCollection<Venue>("venues"); }
        }
    }
}