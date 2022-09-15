using System;
using OpenQA.Selenium;
using System.Threading;
using bot_brainsly_one.src.utils;
using System.Linq;
using OpenQA.Selenium.Support.UI;

namespace bot_brainsly_one.src.actions.instagram
{
    public class Actions
    {
        public void MakeActions(IWebDriver driver)
        {
            try
            {
                var StopTime = DateTime.Now.AddHours(2);
                do
                {
                    Thread.Sleep(2000);

                    bool action = false;

                    IWebElement bodyTag = null;
                    if (driver.TryFindElement(By.TagName("body"), out bodyTag))
                    {
                        IWebElement warningLoading = null;
                        IWebElement warningMaintenance = null;
                        IWebElement buttonReStartSearchActions = null;
                        IWebElement buttonSearchActions = null;
                        IWebElement buttonAccessAction = null;
                        IWebElement buttonStopSearchActions = null;
                        IWebElement buttonActionsScreen = null;

                        IWebElement selectAccount = null;
                        if (driver.TryFindElement(By.XPath("//*[@id='contaig']"), out selectAccount))
                        {
                            SelectElement selectElement = new SelectElement(selectAccount);
                            selectElement.SelectByText($"brainsly_{Program.accountInstagram}");
                            Thread.Sleep(2000);
                        }

                        if (driver.TryFindElement(By.XPath("//b[.='Estamos carregando o sistema, aguarde alguns segundos...']"), out warningLoading) && warningLoading?.Displayed == true)
                        {
                            continue;
                        }
                        else if (driver.TryFindElement(By.XPath("//*[@id='refresh']"), out buttonSearchActions) && buttonSearchActions?.Displayed == true)
                        {
                            buttonSearchActions.Click();
                            continue;
                        }
                        else if (!driver.TryFindElement(By.XPath("//a//b//span[.='Realizar Ações']"), out buttonActionsScreen))
                        {
                            driver.Navigate().Refresh();
                            continue;
                        }
                        else if (driver.TryFindElement(By.XPath("//h1[.='Manutenção Temporária']"), out warningMaintenance))
                        {
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Out.WriteLine("Manutenção Temporária detectada em: " + DateTime.Now + "\n");
                            Console.Out.WriteLine("Suspendendo processo por 5 minutos...");
                            Thread.Sleep(300000);
                            Console.Out.WriteLine("Tentando novamente contatar a plataforma...");
                            driver.Navigate().Refresh();
                            continue;
                        }
                        else if (driver.TryFindElement(By.XPath("//*[@id='btn_iniciar']"), out buttonReStartSearchActions) && buttonReStartSearchActions?.Displayed == true)
                        {
                            buttonReStartSearchActions.Click();
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.Out.WriteLine("Ocorreu um erro, sistema reiniciado em: " + DateTime.Now + "\n");
                            continue;
                        }
                        else
                        {
                            if ((driver.TryFindElement(By.XPath("//*[@id='btn_pausar']"), out buttonStopSearchActions) &&
                                buttonStopSearchActions?.Displayed == true) &&
                                bodyTag?.Text.Contains("Tarefas Esgotadas") == true)
                            {
                                driver.Navigate().Refresh();
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.Out.WriteLine("Ocorreu um erro, sistema reiniciado em: " + DateTime.Now + "\n");
                                continue;
                            }
                        }

                        if (bodyTag?.Text.Contains("Tarefas Esgotadas") == true)
                        {
                            continue;
                        }
                        else if (driver.TryFindElement(By.XPath("//*[@id='btn-acessar']"), out buttonAccessAction))
                        {
                            string type = string.Empty;

                            if (bodyTag?.Text.Contains("Seguir Perfil") == true)
                            {
                                type = "follow";
                            }
                            else if (bodyTag?.Text.Contains("Curtir Publicação") == true)
                            {
                                type = "like";
                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.Out.WriteLine("Novo caso de tarefa detectado: \n\n");
                                Console.Out.WriteLine(bodyTag.Text);
                                Console.Out.WriteLine("\n\n");
                                continue;
                            }

                            buttonAccessAction.Click();
                            Thread.Sleep(5000);
                            Console.ForegroundColor = ConsoleColor.Gray;
                            Console.WriteLine("Tarefa de " + type + " recebida em " + DateTime.Now);

                            driver.SwitchTo().Window(driver.WindowHandles.Last());
                            Thread.Sleep(9000);

                            if (type == "follow") action = this.followUser(driver);
                            else if (type == "like") action = this.likePost(driver);

                            if (action) this.confirmAction(driver, type);
                        }
                    }
                } while (DateTime.Now <= StopTime);
            }
            catch (Exception error)
            {
                Console.Out.WriteLine($"Browser travou com o erro: {error.Message} as {DateTime.Now}");
            }
        }

        private bool likePost(IWebDriver driver)
        {
            try
            {
                IWebElement buttonLike = null;
                if (driver.TryFindElement(By.XPath("//*[@aria-label='Curtir'][name()='svg']/parent::div/parent::button"), out buttonLike))
                {
                    buttonLike.Click();
                    Thread.Sleep(8000);
                    driver.Close();
                    return true;
                }
                else if(driver.TryFindElement(By.XPath("//*[@aria-label='Gosto'][name()='svg']/parent::div/parent::button/parent::span/child::button"), out buttonLike))
                {
                    buttonLike.Click();
                    Thread.Sleep(8000);
                    driver.Close();
                    return true;
                }
                else
                {
                    Thread.Sleep(8000);
                    driver.Close();
                    return false;
                }

            }
            catch (Exception error)
            {
                Console.Out.WriteLine($"cccccccccccccccccc{error.Message}");
                return false;
            }
        }

        private bool followUser(IWebDriver driver)
        {
            try
            {
                IWebElement buttonFollow = null;
                if (driver.TryFindElement(By.XPath("//button[.='Seguir']"), out buttonFollow))
                {
                    buttonFollow.Click();
                    Thread.Sleep(8000);
                    driver.Close();
                    return true;
                }
                else
                {
                    Thread.Sleep(8000);
                    driver.Close();
                    return false;
                }
            }
            catch (Exception error)
            {
                Console.Out.WriteLine($"ddddddddddddddddddddd{error.Message}");
                return false;
            }
        }
        
        private bool confirmAction(IWebDriver driver, string actionType)
        {
            try
            {
                Thread.Sleep(5000);
                driver.SwitchTo().Window(driver.WindowHandles.First());
                Thread.Sleep(3000);

                IWebElement buttonConfirm = null;
                if (driver.TryFindElement(By.XPath("//*[@id='btn-confirmar']"), out buttonConfirm))
                {
                    buttonConfirm.Click();
                    Thread.Sleep(4000);
                    this.LogInfos(actionType);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception error)
            {
                Console.Out.WriteLine($"eeeeeeeeeeeeeeeeeeeeee{error.Message}");
                return false;
            }
        }

        private void LogInfos(string actionType)
        {
            try
            {
                if (actionType == "follow") Program.totalActionsFollowFinished += 1;
                else Program.totalActionsLikeFinished += 1;

                Program.totalActionsFinished += 1;

                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Out.WriteLine("Tarefa finalizada com sucesso em: " + DateTime.Now);
                Console.WriteLine("\n");

                Console.Out.WriteLine("Resumo de Tarefas");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Out.WriteLine("Tarefas de Like realizadas: " + Program.totalActionsLikeFinished);
                Console.Out.WriteLine("Tarefas de Follow realizadas: " + Program.totalActionsFollowFinished);
                Console.WriteLine("\n");

                Console.Out.WriteLine("Total de tarefas realizadas nesse processo: " + Program.totalActionsFinished);
                Console.WriteLine("\n\n");
            }
            catch (Exception error)
            {
                Console.Out.WriteLine($"fffffffffffffffffffff{error.Message}");
            }
        }
    }
}
