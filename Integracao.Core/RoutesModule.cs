using Nancy;
using NLog;

namespace Integracao.Core
{
    public class RoutesModule : NancyModule
    {
        public RoutesModule(Logger log)
        {
            log.Info("Module initialized.");

            Get("/greet/{name}", x => string.Concat("Hello ", x.name), null, "Greet");
        }
    }


}