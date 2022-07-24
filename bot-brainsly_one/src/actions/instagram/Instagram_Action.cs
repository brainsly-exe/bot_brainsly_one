using System;
using OpenQA.Selenium;
using System.Threading;

namespace bot_brainsly_one.src.actions.instagram
{
    public class Instagram_Action
    {
        private IWebDriver foxDriver;

        public Instagram_Action(IWebDriver driver)
        {
            this.foxDriver = driver;
        }
        public bool MakeInstagramAction()
        {
            try
            {
                Thread.Sleep(1000);
                IWebElement buttonSearchActions = this.foxDriver.FindElement(By.XPath("//*[@id='refresh']"));
                buttonSearchActions.Click();

                Thread.Sleep(1000);
                IWebElement headerWithoutTask = this.foxDriver.FindElement(By.XPath("/html/body/div[1]/div/div/div/div/div[5]/div/div/div[1]/h3"));

                if (headerWithoutTask.Text == "Tarefas Esgotadas") return false;

                string id = "raposablaze";

                if (id == "2")
                {
                    this.foxDriver.SwitchTo().NewWindow(WindowType.Tab);
                    Thread.Sleep(1000);

                    return this.likePost(id);
                }
                else
                {
                    this.foxDriver.SwitchTo().NewWindow(WindowType.Tab);
                    Thread.Sleep(1000);

                    return this.followUser(id);
                }
            }
            catch (Exception error)
            {
                throw error;
            }
        }

        private bool likePost(string idPost)
        {
            try
            {
                Thread.Sleep(4000);
                this.foxDriver.Navigate().GoToUrl($"https://www.instagram.com/p/{idPost}/");
                Thread.Sleep(4000);

                IWebElement buttonLike = this.foxDriver.FindElement(By.XPath("/html/body/div[1]/div/div/div/div[1]/div/div/div/div[1]/div[1]/section/main/div[1]/div[1]/article/div/div[2]/div/div[2]/section[1]/span[1]/button"));
                buttonLike.Click();

                Thread.Sleep(1000);
                this.foxDriver.Close();
                return true;
            }
            catch (Exception error)
            {
                Console.Out.WriteLine("erro!" + error); ;
            }

            return false;
        }

        private bool followUser(string idUser)
        {
            try
            {
                Thread.Sleep(4000);
                this.foxDriver.Navigate().GoToUrl($"https://www.instagram.com/{idUser}/");
                Thread.Sleep(4000);

                IWebElement buttonFollow = this.foxDriver.FindElement(By.XPath("/html/body/div[1]/div/div/div/div[1]/div/div/div/div[1]/div[1]/section/main/div/header/section/div[1]/div[1]/div/div[2]/button"));
                buttonFollow.Click();

                Thread.Sleep(1000);
                this.foxDriver.Close();
                return true;
            }
            catch (Exception error)
            {
                Console.Out.WriteLine("erro!" + error); ;
            }
            return false;
        }
    }
}
