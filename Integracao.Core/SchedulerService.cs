using Integracao.Core.Jobs;
using NLog;
using Quartz;
using Quartz.Impl;
using System;
using Topshelf;
using Topshelf.Nancy;

namespace Integracao.Core
{
    public class SchedulerService
    {
        public readonly Logger _log;

        IScheduler scheduler;
        
        public static Host GetHost()
        {
            return HostFactory.New(x =>
            {
                x.UseNLog(new LogFactory(LogManager.Configuration));
                x.Service<SchedulerService>(s =>
                {
                    s.ConstructUsing(name => new SchedulerService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                    s.WithNancyEndpoint(x, c =>
                    {
                        c.AddHost(port: 9000);
                        c.CreateUrlReservationsOnInstall();
                    });
                });

                x.SetDisplayName("Poc-Topshelf+Quartz+Nancy+Core2.2");
                x.SetServiceName("Poc-Topshelf+Quartz+Nancy+Core2.2");

                x.StartAutomatically();
                x.RunAsNetworkService();
            });
        }

        public SchedulerService()
        {
            _log = LogManager.GetLogger("main");

            scheduler = StdSchedulerFactory.GetDefaultScheduler()
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();

            ScheduleJobs();
        }

        private void ScheduleJobs()
        {
            //consultar os jobs
            var jobs = new[] {
                (typeof(Job1).FullName, 5),
                (typeof(Job2).FullName, 5)
            };

            foreach (var job in jobs)
                ScheduleJob(job.Item2, job.FullName);
        }

        private void ScheduleJob(int interval, string fullName) 
        {
            var tipoJob = Type.GetType(fullName);

            var job = JobBuilder.Create(tipoJob)
                                .WithIdentity(fullName)
                                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity($"trigger{tipoJob.Name}")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .RepeatForever()
                    .WithIntervalInSeconds(interval)
                ).Build();

            scheduler.ScheduleJob(job, trigger);

            _log.Info($"ScheduleJob {fullName} - {interval.ToString()}");
        }

        public void Start()
        {
            scheduler.Start();
            _log.Info("Service started");
        }

        public void Stop()
        {
            scheduler.Shutdown();
            _log.Info("Service stoped");
        }

        public void StopJob(JobKey jobKey)
        {
            scheduler.PauseJob(jobKey);
            _log.Info($"Job stoped {jobKey.Name}");
        }

        public void RescheduleJob(JobKey jobKey, int interval)
        {
            scheduler.DeleteJob(jobKey);

            ScheduleJob(interval, jobKey.Name);

            _log.Info($"Job rescheduled {jobKey.Name}, {interval.ToString()}");
        }

    }


}