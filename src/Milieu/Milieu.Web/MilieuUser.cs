using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Security;

namespace Milieu.Web
{
    public class MilieuUser : IUserIdentity
    {
        private readonly string _email;
        private readonly IReadOnlyList<string> _claims;

        public MilieuUser(string email, IEnumerable<string> claims)
        {
            _email = email;
            _claims = claims.ToList();
        }

        public string UserName
        {
            get { return _email; }
        }

        public IEnumerable<string> Claims
        {
            get { return _claims; }
        }
    }
}