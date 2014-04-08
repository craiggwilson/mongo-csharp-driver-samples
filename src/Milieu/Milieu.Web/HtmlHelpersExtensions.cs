using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Nancy.ViewEngines.Razor;

namespace Milieu.Web
{
    public static class HtmlHelpersExtensions
    {
        public static string UserGravatar<T>(this HtmlHelpers<T> html, string email)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(email));
                var sb = new StringBuilder("http://www.gravatar.com/avatar/");
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("x2"));
                }
                sb.Append(".png");
                return sb.ToString();
            }
        }

        public static string CurrentUserGravatar<T>(this HtmlHelpers<T> html)
        {
            return UserGravatar(html, html.CurrentUser.UserName);
        }
    }
}