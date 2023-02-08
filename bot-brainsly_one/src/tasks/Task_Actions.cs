using System.Threading.Tasks;
using Quartz;
using bot_brainsly_one.src.actions.instagram;
using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using bot_brainsly_one.src.utils;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Linq;
using System.Data;
using System.Web.UI.WebControls;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using bot_brainsly_one.src.DTO;

namespace bot_brainsly_one.src.tasks
{
    [DisallowConcurrentExecution]
    public class Task_Actions : IJob
    {
        public IWebDriver driver;
        public bool isLogged = false;

        public Task Execute(IJobExecutionContext context)
        {
            var StopTime = DateTime.Now.AddHours(Program.auxActualRemainingProcessHours);

            try
            {
                do
                {
                    this.StartWebDriver();
                    this.StartProcess();

                    new Actions().MakeActions(this.driver);
                    this.driver.Quit();
                    this.isLogged = false;
                } while (DateTime.Now <= StopTime);

                Program.auxActualRemainingProcessHours = 20;
                return Task.CompletedTask;
            }
            catch (Exception error)
            {
                Console.Out.WriteLine($"Error: { error.Message}");
                this.driver.Quit();
                this.isLogged = false;

                Program.auxActualRemainingProcessHours = (StopTime.Subtract(DateTime.Now).Hours);

                return Execute(context);
            }
        }

        private void StartWebDriver()
        {
            try
            {
                string correctProfile = this.ChooseChromeProfile();

                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--disable-dev-shm-usage");
                options.AddArgument("--disable-browser-side-navigation");
                //options.AddArgument("--headless");
                //options.AddArgument("--no-sandbox");
                //options.AddArgument("--disable-gpu");
                options.AddArgument("--start-maximized");
                options.AddArgument("--window-size=1920,1080");
                options.AddArgument("--hide-scrollbars");
                options.AddArgument("--v=99");
                options.AddArgument("--single-process");
                options.AddArgument("--data-path=/tmp/data-path");
                options.AddArgument("--ignore-certificate-errors");
                options.AddArgument("--homedir=/tmp");
                options.AddArgument("--disk-cache-dir=/tmp/cache-dir");
                options.AddArgument($"user-data-dir={new FileUtils().BaseProjectDirectory}\\profiles\\{Program.accountInstagram}\\User Data");
                options.AddArgument($"profile-directory={correctProfile}");
                options.AddArgument("user-agent=Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/105.0.0.0 Safari/537.36");
                options.AddExcludedArguments("enable-automation", "enable-logging");

                this.driver = new ChromeDriver(options);
            }
            catch (Exception error)
            {
                throw error;
            }
        }

        private string ChooseChromeProfile()
        {
            var userDataPath = $"{new FileUtils().BaseProjectDirectory}\\profiles\\{Program.accountInstagram}\\User Data";
            var userDataFolders = new DirectoryInfo(userDataPath).GetDirectories("Profile*");

            foreach (DirectoryInfo directory in userDataFolders)
            {
                IEnumerable<FileInfo> fileList = directory.GetFiles("Preferences");

                using (StreamReader file = File.OpenText(fileList.First().FullName))
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    ChromeConfigFileDTO chromeConfig = serializer.Deserialize<ChromeConfigFileDTO>(reader);

                    if (chromeConfig.profile.name == Program.accountInstagram) return directory.Name;
                }
            }

            throw new Exception("Profile não localizado para o bot.");
        }

        private void StartProcess()
        {
            this.AcessGanharNasRedes();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("Iniciado processo com sucesso em " + DateTime.Now + "\n\n");
        }

        private bool AcessGanharNasRedes()
        {
            while (!this.isLogged)
            {
                Thread.Sleep(5000);
                this.driver.Navigate().GoToUrl("https://www.ganharnasredes.com/painel/");
                Thread.Sleep(10000);

                IWebElement buttonActionsScreen = null;
                if (this.driver.TryFindElement(By.XPath("//a//b//span[.='Realizar Ações']"), out buttonActionsScreen))
                {
                    buttonActionsScreen.Click();
                    Thread.Sleep(10000);
                }

                IWebElement selectAccount = null;
                if (driver.TryFindElement(By.XPath("//*[@id='contaig']"), out selectAccount))
                {
                    SelectElement selectElement = new SelectElement(selectAccount);
                    selectElement.SelectByText($"brainsly_{Program.accountInstagram}");
                    Thread.Sleep(4000);
                }

                IWebElement buttonStartSystemActions = null;
                if (this.driver.TryFindElement(By.XPath("//*[@id='btn_iniciar']"), out buttonStartSystemActions))
                {
                    buttonStartSystemActions.Click();
                    Thread.Sleep(4000);
                    this.isLogged = true;
                }
            }
            return true;
        }
    }
}
