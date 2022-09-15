using bot_brainsly_one.src.jobs;
using System;

namespace bot_brainsly_one
{
    public class Program
    {
        public static string accountInstagram; 

        public static int totalActionsFinished = 0;
        public static int totalActionsLikeFinished = 0;
        public static int totalActionsFollowFinished = 0;

        public static void Main(string[] args)
        {
            Console.Out.Write("Insira qual conta do Instagram deseja usar: ");
            Program.accountInstagram = Console.ReadLine();
          
            Console.Title = $"Brainsly_{Program.accountInstagram}";
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"-- Iniciado o BOT Brainsly_{Program.accountInstagram} --");
            Console.WriteLine("\n\n\n");
            Console.ForegroundColor = ConsoleColor.White;

            new Execute_Bot().ExecuteBot();
        }
    }
}
