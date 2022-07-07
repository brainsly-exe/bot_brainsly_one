using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using bot_brainsly_one.src.actions.instagram;

namespace bot_brainsly_one.src.tasks
{
    public class Task_Instagram: IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            await new Instagram_Action().MakeInstagramAction();
        }
    }
}
