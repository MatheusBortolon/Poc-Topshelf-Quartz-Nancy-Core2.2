using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using NLog;

namespace Integracao.Core
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);

            container.Register<Logger>(LogManager.GetLogger("main"));
        }

    }

}