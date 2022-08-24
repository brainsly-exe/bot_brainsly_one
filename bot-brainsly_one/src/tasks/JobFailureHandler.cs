using Quartz;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace bot_brainsly_one.src.tasks
{
    public class JobFailureHandler : IJobListener
    {
        public string Name => "FailJobListener";

        public Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            if (jobException == null)
            {
                return Task.CompletedTask;
            }

            Console.WriteLine("Job foi reagendado!");
            context.Scheduler.RescheduleJob(context.Trigger.Key, context.Trigger).ConfigureAwait(false).GetAwaiter().GetResult();
            return Task.CompletedTask;
        }
    }
}
