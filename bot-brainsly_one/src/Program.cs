using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bot_brainsly_one.src.jobs;

namespace bot_brainsly_one
{
    public class Program
    {
        private static async Task Main(string[] args)
        {
            Console.WriteLine("---------------|   Iniciado o BOT BRAINSLT_ONE   |--------------------");
            await new Execute_Bot().ExecuteBot();
        }
    }
}
