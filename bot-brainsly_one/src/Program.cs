using bot_brainsly_one.src.jobs;
using System;
using System.Collections.Generic;

namespace bot_brainsly_one
{
    public class Program
    {
        public static string accountInstagram;

        public static List<string> accountInstagramGroup1 = new List<string>() { "one", "three", "four", "five", "six" };
        public static List<string> accountInstagramGroup2 = new List<string>() { "fiveteen", "sixteen", "seventeen", "eighteen", "nineteen" };

        public static int totalActionsFinished = 0;
        public static int totalActionsLikeFinished = 0;
        public static int totalActionsFollowFinished = 0;

        public static int auxActualRemainingProcessHours = 20;

        public static void Main(string[] args)
        {
            Program.accountInstagram = args[0];
          
            Console.Title = $"Brainsly_{Program.accountInstagram}";
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"-- Iniciado o BOT Brainsly_{Program.accountInstagram} --");
            Console.WriteLine("\n\n\n");
            Console.ForegroundColor = ConsoleColor.White;

            new Execute_Bot().ExecuteBot();
        }
    }
}
