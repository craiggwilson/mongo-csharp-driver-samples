using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Milieu.Web.Domain;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Security;

namespace Milieu.Web
{
    public class MilieuUserMapper : IUserMapper
    {
        private readonly MilieuDataContext _context;

        public MilieuUserMapper(MilieuDataContext context)
        {
            _context = context;
        }

        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            var user = _context.Users.AsQueryable().SingleOrDefault(x => x.Id == identifier);
            if (user == null)
                return null;

            return new MilieuUser(user.Email, user.Claims);
        }
    }
}