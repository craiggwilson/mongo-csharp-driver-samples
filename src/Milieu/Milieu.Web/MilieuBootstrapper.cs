using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Milieu.Web.Domain;
using MongoDB.Driver;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.ViewEngines.Razor;

namespace Milieu.Web
{
    public class MilieuBootstrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureApplicationContainer(Nancy.TinyIoc.TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            var db = new MongoClient()
                .GetServer()
                .GetDatabase("milieu");

            container.Register(new MilieuDataContext(db));
        }

        protected override void ConfigureRequestContainer(Nancy.TinyIoc.TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
            container.Register<IUserMapper, MilieuUserMapper>();
        }

        protected override void RequestStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);

            var formsAuthConfig = new FormsAuthenticationConfiguration
            {
                RedirectUrl = "~/login",
                UserMapper = container.Resolve<IUserMapper>()
            };
            FormsAuthentication.Enable(this.ApplicationPipelines, formsAuthConfig);
        }
    }
}