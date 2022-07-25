using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("---------------|   Iniciado o BOT BRAINSLT_ONE   |--------------------");
            Console.WriteLine("\n\n\n");
            Console.ForegroundColor = ConsoleColor.White;

            Program.MakeLogins();

            await new Execute_Bot().ExecuteBot();
        }

        private static void MakeLogins()
        {
            Program.foxDriver = new FirefoxDriver();
            if (Program.loginInstagram())
            {
                Console.WriteLine("\n\n\n");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Efetuado Login no Instagram com sucesso em " + DateTime.Now);
                if(Program.loginGanharNoInsta())
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Efetuado Login no GanharNoInsta com sucesso em " + DateTime.Now + "\n\n");
                    Program.isLogged = true;
                }
            }
        }

        private static bool loginInstagram()
        {
            try
            {
                Program.foxDriver.Navigate().GoToUrl("http://www.instagram.com");
                Thread.Sleep(2000);

                IWebElement email = Program.foxDriver.FindElement(By.CssSelector("input[name='username']"));
                email.SendKeys("brainsly.one@gmail.com");

                IWebElement password = Program.foxDriver.FindElement(By.CssSelector("input[name='password']"));
                password.SendKeys("147258369Pedro");

                password.Submit();

                return true;
            }
            catch (Exception)
            {
                return false; ;
            }
        }

        private static bool loginGanharNoInsta()
        {
            try
            {
                Thread.Sleep(4000);
                Program.foxDriver.Navigate().GoToUrl("https://www.ganharnoinsta.com/painel/");
                Thread.Sleep(2000);

                IWebElement email = Program.foxDriver.FindElement(By.CssSelector("input[name='email']"));
                email.SendKeys("brainsly.one@gmail.com");

                IWebElement password = Program.foxDriver.FindElement(By.CssSelector("input[name='senha']"));
                password.SendKeys("147258369Pedro");

                password.Submit();
                Thread.Sleep(4000);

                IWebElement buttonActionsScreen = Program.foxDriver.FindElement(By.XPath("/html/body/div[1]/aside/div/nav/ul/li[9]/center/a"));
                buttonActionsScreen.Click();

                Thread.Sleep(4000);
                IWebElement buttonStartSystemActions = Program.foxDriver.FindElement(By.XPath("//*[@id='btn_iniciar']"));
                buttonStartSystemActions.Click();

                return true;
            }
            catch (Exception)
            {
                return false; ;
            }
        }
    }
}
