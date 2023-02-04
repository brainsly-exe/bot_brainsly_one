using System;
using OpenQA.Selenium;
using System.Threading;
using bot_brainsly_one.src.utils;
using System.Linq;
using OpenQA.Selenium.Support.UI;
using System.Data;

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
                                Console.Out.WriteLine("Ocorreu um erro na plataforma, processo reiniciado em: " + DateTime.Now + "\n");
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
                            //Console.ForegroundColor = ConsoleColor.Gray;
                            //Console.WriteLine("Tarefa de " + type + " recebida em " + DateTime.Now);

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
                throw error;
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

                    IWebElement bodyTag = null;
                    if (driver.TryFindElement(By.TagName("body"), out bodyTag))
                    {
                       if(bodyTag?.Text.Contains("A tua conta foi bloqueada temporariamente e não pode efetuar esta ação") == true)
                       {
                            driver.Quit();
                            Environment.Exit(0);
                       }
                       else
                       {
                            driver.Close();
                            return true;
                       }
                    }

                    return false;
                }
                else if(driver.TryFindElement(By.XPath("//*[@aria-label='Gosto'][name()='svg']/parent::div/parent::button/parent::span/child::button"), out buttonLike))
                {
                    buttonLike.Click();
                    Thread.Sleep(8000);

                    IWebElement bodyTag = null;
                    if (driver.TryFindElement(By.TagName("body"), out bodyTag))
                    {
                        if (bodyTag?.Text.Contains("A tua conta foi bloqueada temporariamente e não pode efetuar esta ação") == true)
                        {
                            driver.Quit();
                            Environment.Exit(0);
                        }
                        else
                        {
                            driver.Close();
                            return true;
                        }
                    }

                    return false;
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
                throw error;
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

                    IWebElement bodyTag = null;
                    if (driver.TryFindElement(By.TagName("body"), out bodyTag))
                    {
                        if (bodyTag?.Text.Contains("A tua conta foi bloqueada temporariamente e não pode efetuar esta ação") == true)
                        {
                            driver.Quit();
                            Environment.Exit(0);
                        }
                        else
                        {
                            driver.Close();
                            return true;
                        }
                    }

                    return false;
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
                throw error;
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
                    this.UpdateProcessStatus(actionType);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception error)
            {
                throw error;
            }
        }

        private void UpdateProcessStatus(string actionType)
        {
            try
            {
                if (actionType == "follow")
                {
                    Program.totalActionsFollowFinished += 1;
                }
                else
                {
                    Program.totalActionsLikeFinished += 1;
                }

                Program.totalActionsFinished += 1;

                DataSet dataSet = new DataSet();
                dataSet.Tables.Add("report");
                dataSet.Tables["report"].Columns.Add("A");
                dataSet.Tables["report"].Columns.Add("B");
                dataSet.Tables["report"].Columns.Add("C");
                dataSet.Tables["report"].Columns.Add("D");
                dataSet.Tables["report"].Columns.Add("E");
                dataSet.Tables["report"].Columns.Add("F");

                dataSet.Tables["report"].Columns["F"].DataType = typeof(Double);
                dataSet.Tables["report"].Rows.Add();

                dataSet.Tables["report"].Rows[0]["A"] = "Instagram";
                dataSet.Tables["report"].Rows[0]["B"] = Program.totalActionsLikeFinished;
                dataSet.Tables["report"].Rows[0]["C"] = Program.totalActionsFollowFinished;
                dataSet.Tables["report"].Rows[0]["D"] = Program.totalActionsFinished;
                dataSet.Tables["report"].Rows[0]["E"] = DateTime.Now.ToString("dd/MM/yyyy");
                dataSet.Tables["report"].Rows[0]["F"] = (Program.totalActionsLikeFinished * 0.002) + (Program.totalActionsFollowFinished * 0.004);
                //0,002 = like
                //0,004 = follow

                new ReportUtils().ExportDataSet(dataSet);
            }
            catch (Exception error)
            {
                throw error;
            }
        }
    }
}
