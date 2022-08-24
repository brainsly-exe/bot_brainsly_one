using System.Threading.Tasks;
using Quartz;
using bot_brainsly_one.src.actions.instagram;
using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using bot_brainsly_one.src.utils;

namespace bot_brainsly_one.src.tasks
{
    [DisallowConcurrentExecution]
    public class Task_Actions : IJob
    {
        public IWebDriver driver;
        public bool isLogged = false;

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                this.StartWebDriver();
                this.MakeLogins();

                if (this.isLogged)
                {
                    new Actions().MakeActions(this.driver);
                    this.driver.Quit();
                }
                else
                {
                    this.driver.Quit();
                    Environment.Exit(0);
                }
                return Task.CompletedTask;
            }
            catch (Exception error)
            {
                Console.Out.WriteLine(error.Message);
                return Task.CompletedTask;
            }
        }

        private void StartWebDriver()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument("enable-automation");
            options.AddArgument("--disable-dev-shm-usage");
            options.AddArgument("--disable-browser-side-navigation");
            options.AddArgument("--headless");
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-gpu");
            options.AddArgument("--window-size=1280x1696");
            options.AddArgument("--hide-scrollbars");
            options.AddArgument("--v=99");
            options.AddArgument("--single-process");
            options.AddArgument("--data-path=/tmp/data-path");
            options.AddArgument("--ignore-certificate-errors");
            options.AddArgument("--homedir=/tmp");
            options.AddArgument("--disk-cache-dir=/tmp/cache-dir");
            options.AddArgument("user-agent=Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.100 Safari/537.36");
            options.AddExcludedArguments("enable-automation", "enable-logging");

            this.driver = new ChromeDriver(options);
        }

        private void MakeLogins()
        {
            if (this.loginInstagram())
            {
                Console.WriteLine("\n\n\n");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.WriteLine("Efetuado Login no Instagram com sucesso em " + DateTime.Now);
                if (this.loginGanharNoInsta())
                {
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine("Efetuado Login no GanharNoInsta com sucesso em " + DateTime.Now + "\n\n");
                    this.isLogged = true;
                }
            }
        }

        private bool loginInstagram()
        {
            try
            {
                this.driver.Navigate().GoToUrl("http://www.instagram.com");
                Thread.Sleep(3000);

                IWebElement email = this.driver.FindElement(By.CssSelector("input[name='username']"));
                email.SendKeys($"brainsly_{Program.accountInstagram}");

                IWebElement password = this.driver.FindElement(By.CssSelector("input[name='password']"));
                password.SendKeys("147258369Pedro");

                password.Submit();

                Thread.Sleep(7000);

                IWebElement imageAvatar = null;
                if (this.driver.TryFindElement(By.XPath("//img[@data-testid='user-avatar']"), out imageAvatar))
                {
                    return true;
                }

                return false;
            }
            catch (Exception error)
            {
                Console.Out.WriteLine(error.Message);
                return false;
            }
        }

        private bool loginGanharNoInsta()
        {
            try
            {
                Thread.Sleep(5000);
                this.driver.Navigate().GoToUrl("https://www.ganharnoinsta.com/painel/");
                Thread.Sleep(6000);

                IWebElement buttonCloseModalWarning = null;
                if (this.driver.TryFindElement(By.XPath("//*[@id='modalAvisoCurso']/div/div/div[3]/button"), out buttonCloseModalWarning))
                {
                    buttonCloseModalWarning.Click();
                    Thread.Sleep(2000);
                }

                IWebElement email = this.driver.FindElement(By.CssSelector("input[name='email']"));
                email.SendKeys($"brainsly.{Program.botName}@gmail.com");

                IWebElement password = this.driver.FindElement(By.CssSelector("input[name='senha']"));
                password.SendKeys("147258369Pedro");

                password.Submit();
                Thread.Sleep(10000);

                IWebElement buttonActionsScreen = null;
                if (this.driver.TryFindElement(By.XPath("//a//b//span[.='Realizar Ações']"), out buttonActionsScreen))
                {
                    buttonActionsScreen.Click();
                    Thread.Sleep(7000);
                }

                IWebElement buttonStartSystemActions = null;
                if (this.driver.TryFindElement(By.XPath("//*[@id='btn_iniciar']"), out buttonStartSystemActions))
                {
                    buttonStartSystemActions.Click();
                    Thread.Sleep(3000);
                    return true;
                }

                return false;
            }
            catch (Exception error)
            {
                Console.Out.WriteLine(error.Message);
                return false;
            }
        }
    }
}
