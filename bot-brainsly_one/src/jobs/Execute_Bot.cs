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

                ITrigger trigger;

                if (Program.accountInstagramGroup1.Contains(Program.accountInstagram))
                {
                    trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger3", "group1")
                    .StartAt(DateBuilder.TodayAt(4, 10, 0))
                    .WithSimpleSchedule(x =>
                            x.WithIntervalInHours(2 * 24)
                            .RepeatForever()
                    ).Build();
                }
                else
                {
                    trigger = TriggerBuilder.Create()
                    .WithIdentity("trigger3", "group1")
                    .StartAt(DateBuilder.TomorrowAt(4, 15, 0))
                    .WithSimpleSchedule(x =>
                            x.WithIntervalInHours(2 * 24)
                            .RepeatForever()
                    ).Build();
                }

                // DESATIVADO
                //ITrigger trigger = TriggerBuilder.Create()
                //    .WithIdentity("trigger1", "group1")
                //    .StartNow()
                //    .WithSimpleSchedule(x => x.WithIntervalInSeconds(1).RepeatForever())
                //    .Build();

                // Tell quartz to schedule the job using our trigger
                scheduler.ScheduleJob(job, trigger).ConfigureAwait(false).GetAwaiter().GetResult();

                Console.ReadKey();
            }
            catch (Exception error)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Out.WriteLine($"Error: {error.Message}");
            }
        }
    }
}
