using System.Threading.Tasks;
using Quartz;
using bot_brainsly_one.src.actions.instagram;
using System;

namespace bot_brainsly_one.src.tasks
{
    [DisallowConcurrentExecution]
    public class Task_Instagram : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            if (Program.isLogged)
            {
                new Instagram_Action(Program.foxDriver).MakeInstagramAction();
            }
            return Task.CompletedTask;
        }
    }
}
