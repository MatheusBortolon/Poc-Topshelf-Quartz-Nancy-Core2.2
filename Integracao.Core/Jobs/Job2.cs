using NLog;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Integracao.Core.Jobs
{
    public class Job2 : IJob
    {
        public Job2()
        {
        }

        public Task Execute(IJobExecutionContext context) =>
            Task.Run(() => LogManager.GetLogger("main").Info($"{DateTime.Now.ToString()} - Job2"));

    }
}