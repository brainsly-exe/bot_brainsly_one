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
                Thread.Sleep(500);
                IWebElement buttonReStartSearchActions = null;
                if (this.foxDriver.TryFindElement(By.XPath("//*[@id='btn_iniciar']"), out buttonReStartSearchActions) && buttonReStartSearchActions?.Displayed == true)
                {
                    buttonReStartSearchActions.Click();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Out.WriteLine("Ocorreu um erro, sistema reiniciado em: " + DateTime.Now + "\n");
                    return false;
                }

                IWebElement buttonSearchActions = null;
                if (this.foxDriver.TryFindElement(By.XPath("//*[@id='refresh']"), out buttonSearchActions))
                {
                    buttonSearchActions.Click();
                }

                bool action = false;
                Thread.Sleep(3000);
                IWebElement bodyTag = null;
                if (this.foxDriver.TryFindElement(By.TagName("body"), out bodyTag))
                {
                    IWebElement buttonAccessAction = null;

                    IWebElement buttonStopSearchActions = null;
                    if
                        (
                            (this.foxDriver.TryFindElement(By.XPath("//*[@id='btn_pausar']"), out buttonStopSearchActions) && buttonStopSearchActions?.Displayed == true) &&
                            buttonSearchActions == null &&
                            bodyTag?.Text.Contains("Tarefas Esgotadas") == true
                        )
                    {
                        this.foxDriver.Navigate().Refresh();
                        Console.Out.WriteLine("Ocorreu um erro, sistema reiniciado em: " + DateTime.Now + "\n");
                        return false;
                    }

                    IWebElement buttonActionsScreen = null;
                    if(!this.foxDriver.TryFindElement(By.XPath("//a//b//span[.='Realizar Ações']"), out buttonActionsScreen))
                    {
                        this.foxDriver.Navigate().Refresh();
                        return false;
                    }


                    if (bodyTag?.Text.Contains("Tarefas Esgotadas") == true)
                    {
                        return false;
                    }
                    else if(this.foxDriver.TryFindElement(By.XPath("//*[@id='btn-acessar']"), out buttonAccessAction))
                    {
                        string type = string.Empty;

                        if(bodyTag?.Text.Contains("Seguir Perfil") == true)
                        {
                            type = "follow";
                        }
                        else if(bodyTag?.Text.Contains("Curtir Publicação") == true)
                        {
                            type = "like";
                        }
                        else
                        {
                            return false;
                        }

                        buttonAccessAction.Click();

                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.WriteLine("Tarefa de " + type + " recebida em " + DateTime.Now);

                        if (type == "follow") action = this.followUser();
                        else if (type == "like") action = this.likePost();

                        if (action) return this.confirmAction(type);

                    }
                }

                return false;
            }
            catch (Exception)
            {
                this.foxDriver.Navigate().Refresh();
                Console.Out.WriteLine("Ocorreu um erro, sistema reiniciado em: " + DateTime.Now + "\n");
                return false;
            }
        }

        private bool likePost()
        {
            try
            {
                Thread.Sleep(11000);
                this.foxDriver.SwitchTo().Window(this.foxDriver.WindowHandles.Last());

                IWebElement buttonLike = null;
                if (this.foxDriver.TryFindElement(By.XPath("//*[@aria-label='Curtir'][name()='svg']/parent::div/parent::button"), out buttonLike))
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
                if (this.foxDriver.TryFindElement(By.XPath("//button[.='Seguir']"), out buttonFollow))
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
            catch (Exception)
            {
                return false;
            }
        }

        private bool confirmAction(string actionType)
        {
            try
            {
                Thread.Sleep(5000);
                this.foxDriver.SwitchTo().Window(this.foxDriver.WindowHandles.First());

                IWebElement buttonConfirm = null;
                if (this.foxDriver.TryFindElement(By.XPath("//*[@id='btn-confirmar']"), out buttonConfirm))
                {
                    buttonConfirm.Click();
                    Thread.Sleep(4000);

                    this.LogInfosInstagram(actionType);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void LogInfosInstagram(string actionType)
        {
            if (actionType == "follow")
            {
                Program.totalActionsFollowFinishedInstagram += 1;
            }
            else
            {
                Program.totalActionsLikeFinishedInstagram += 1;
            }

            Program.totalActionsFinishedInstagram += 1;

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.Out.WriteLine("Tarefa finalizada com sucesso em: " + DateTime.Now);
            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Out.WriteLine("Tarefas de Like realizadas: " + Program.totalActionsLikeFinishedInstagram);
            Console.Out.WriteLine("Tarefas de Follow realizadas: " + Program.totalActionsFollowFinishedInstagram);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Out.WriteLine("Total de tarefas realizadas nesse processo: " + Program.totalActionsFinishedInstagram);
            Console.WriteLine("\n\n");
        }
    }
}
