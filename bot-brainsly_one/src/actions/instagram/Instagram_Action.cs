using System;
using OpenQA.Selenium;
using System.Threading;
using bot_brainsly_one.src.utils;
using System.Linq;

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
                Thread.Sleep(3000);
                IWebElement buttonSearchActions = null;
                if (this.foxDriver.TryFindElement(By.XPath("//*[@id='refresh']"), out buttonSearchActions))
                {
                    buttonSearchActions.Click();
                }

                bool action = false;
                Thread.Sleep(3000);
                IWebElement headerWithoutTask = null;
                if (this.foxDriver.TryFindElement(By.XPath("/html/body/div[1]/div/div/div/div/div[5]/div/div/div[1]/h3"), out headerWithoutTask))
                {
                    if (headerWithoutTask?.Text == "Tarefas Esgotadas")
                    {
                        return false;
                    }
                }
                else
                {
                    IWebElement infoActionType = null;
                    if (this.foxDriver.TryFindElement(By.XPath("/html/body/div[1]/div/div/div/div/div[5]/center/div/div/div[1]/b"), out infoActionType))
                    {
                        IWebElement buttonAccessAction = null;
                        if (this.foxDriver.TryFindElement(By.XPath("//*[@id='btn-acessar']"), out buttonAccessAction))
                        {
                            string type = infoActionType?.Text.Contains("Seguir Perfil") == true ? "follow" : "like";

                            buttonAccessAction.Click();

                            if (type == "follow") action = this.followUser();
                            else if (type == "like") action = this.likePost();

                            if (action) return this.confirmAction();
                        }
                    }
                }
                return false;
            }
            catch (NoSuchElementException){ }
            catch (Exception error)
            {
                throw error;
            }

            return false;
        }

        private bool likePost()
        {
            try
            {
                Thread.Sleep(11000);
                this.foxDriver.SwitchTo().Window(this.foxDriver.WindowHandles.Last());

                IWebElement buttonLike = null;
                if (this.foxDriver.TryFindElement(By.XPath("/html/body/div[1]/div/div/div/div[1]/div/div/div/div[1]/div[1]/section/main/div[1]/div[1]/article/div/div[2]/div/div[2]/section[1]/span[1]/button"), out buttonLike))
                {
                    buttonLike.Click();
                    Thread.Sleep(8000);
                    this.foxDriver.Close();
                    return true;
                }

                Thread.Sleep(8000);
                this.foxDriver.Close();
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool followUser()
        {
            try
            {
                Thread.Sleep(11000);
                this.foxDriver.SwitchTo().Window(this.foxDriver.WindowHandles.Last());

                IWebElement buttonFollow = null;
                if (this.foxDriver.TryFindElement(By.XPath("/html/body/div[1]/div/div/div/div[1]/div/div/div/div[1]/div[1]/section/main/div/header/section/div[1]/div[1]/div/div[2]/button"), out buttonFollow))
                {
                    buttonFollow.Click();
                    Thread.Sleep(8000);
                    this.foxDriver.Close();
                    return true;
                }

                Thread.Sleep(8000);
                this.foxDriver.Close();
                return false;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool confirmAction()
        {
            try
            {
                Thread.Sleep(5000);
                this.foxDriver.SwitchTo().Window(this.foxDriver.WindowHandles.First());
                IWebElement buttonConfirm = this.foxDriver.FindElement(By.XPath("//*[@id='btn-confirmar']"));
                buttonConfirm.Click();
                Thread.Sleep(4000);

                return true;
            }
            catch (NoSuchElementException error) 
            {
                throw error;
            }
            catch (Exception error)
            {
                throw error;
            }
        }
    }
}
