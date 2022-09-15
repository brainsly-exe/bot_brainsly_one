using System;
using Quartz;
using Quartz.Impl;
using bot_brainsly_one.src.tasks;

namespace bot_brainsly_one.src.jobs
{
    public class Execute_Bot
    {
        public void ExecuteBot()
        {
            try
            {
                // Grab the Scheduler instance from the Factory
                StdSchedulerFactory factory = new StdSchedulerFactory();
                IScheduler scheduler = factory.GetScheduler().ConfigureAwait(false).GetAwaiter().GetResult();

                // and start it off
                scheduler.Start().ConfigureAwait(false).GetAwaiter().GetResult();

                // define the job and tie it to our HelloJob class
                IJobDetail job = JobBuilder.Create<Task_Actions>()
                    .WithIdentity("job1", "group1")
                    .Build();

                // Trigger the job to run now, and then repeat every 6 seconds
                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger1", "group1")
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithIntervalInSeconds(1).RepeatForever())
                    .Build();

                // Tell quartz to schedule the job using our trigger
                scheduler.ScheduleJob(job, trigger).ConfigureAwait(false).GetAwaiter().GetResult();

                Console.ReadKey();
            }
            catch (Exception error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine($"hhhhhhhhhhhhhhh{error.Message}");
            }
        }
    }
}
