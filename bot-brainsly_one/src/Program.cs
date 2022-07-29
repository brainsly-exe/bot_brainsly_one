using System;
using System.Threading;
using System.Threading.Tasks;
using bot_brainsly_one.src.jobs;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace bot_brainsly_one
{
    public class Program
    {
        public static IWebDriver foxDriver;
        public static bool isLogged = false;

        public static int totalActionsFinishedInstagram = 0;
        public static int totalActionsLikeFinishedInstagram = 0;
        public static int totalActionsFollowFinishedInstagram = 0;

        private static async Task Main(string[] args)
        {
            Console.Out.Write("Insira qual bot deseja startar: ");
            string botName = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"-- Iniciado o BOT BRAINSLY_{botName.ToUpper()} --");
            Console.WriteLine("\n\n\n");
            Console.ForegroundColor = ConsoleColor.White;

            Program.MakeLogins(botName);

            await new Execute_Bot().ExecuteBot();
        }

        private static void MakeLogins(string botName)
        {
            Program.foxDriver = new FirefoxDriver();
            if (Program.loginInstagram(botName))
            {
                Console.WriteLine("\n\n\n");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Efetuado Login no Instagram com sucesso em " + DateTime.Now);
                if(Program.loginGanharNoInsta(botName))
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Efetuado Login no GanharNoInsta com sucesso em " + DateTime.Now + "\n\n");
                    Program.isLogged = true;
                }
            }
        }

        private static bool loginInstagram(string botName)
        {
            try
            {
                Program.foxDriver.Navigate().GoToUrl("http://www.instagram.com");
                Thread.Sleep(2000);

                IWebElement email = Program.foxDriver.FindElement(By.CssSelector("input[name='username']"));
                email.SendKeys($"brainsly.{botName}@gmail.com");

                IWebElement password = Program.foxDriver.FindElement(By.CssSelector("input[name='password']"));
                password.SendKeys("147258369Pedro");

                password.Submit();

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool loginGanharNoInsta(string botName)
        {
            try
            {
                Thread.Sleep(4000);
                Program.foxDriver.Navigate().GoToUrl("https://www.ganharnoinsta.com/painel/");
                Thread.Sleep(2000);

                IWebElement email = Program.foxDriver.FindElement(By.CssSelector("input[name='email']"));
                email.SendKeys($"brainsly.{botName}@gmail.com");

                IWebElement password = Program.foxDriver.FindElement(By.CssSelector("input[name='senha']"));
                password.SendKeys("147258369Pedro");

                password.Submit();
                Thread.Sleep(8000);

                IWebElement buttonActionsScreen = Program.foxDriver.FindElement(By.XPath("//a//b//span[.='Realizar Ações']"));
                buttonActionsScreen.Click();

                Thread.Sleep(4000);
                IWebElement buttonStartSystemActions = Program.foxDriver.FindElement(By.XPath("//*[@id='btn_iniciar']"));
                buttonStartSystemActions.Click();
                Thread.Sleep(3000);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
