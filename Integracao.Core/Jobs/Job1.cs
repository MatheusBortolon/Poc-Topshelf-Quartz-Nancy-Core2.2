using NLog;
using Quartz;
using System;
using System.Threading.Tasks;

namespace Integracao.Core.Jobs
{
    public class Job1 : IJob
    {
        public Job1()
        {
        }

        public Task Execute(IJobExecutionContext context) =>
            Task.Run(() => LogManager.GetLogger("main").Info($"{DateTime.Now.ToString()} - Job1"));

    }
}