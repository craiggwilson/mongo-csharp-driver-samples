using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nancy.Security;

namespace Milieu.Web.Models
{
    public class UserModel : IUserIdentity
    {
        public string UserName
        {
            get { throw new NotImplementedException(); }
        }

        public string UserNameHash
        {
            get { throw new NotImplementedException(); }
        }

        public IEnumerable<string> Claims
        {
            get { throw new NotImplementedException(); }
        }
    }
}
