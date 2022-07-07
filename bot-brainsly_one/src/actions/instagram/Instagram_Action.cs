using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Newtonsoft.Json;
using bot_brainsly_one.src.models;

namespace bot_brainsly_one.src.actions.instagram
{
    public class Instagram_Action
    {
        public Instagram_Action()
        {

        }
        public async Task MakeInstagramAction()
        {
            HttpClient client = new HttpClient();

            var payload = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "id_contaig", "15799597" },
                    { "id_contayt", "" },
                    { "id_contakwai", "" },
                    { "id_contatt", "" },
                    { "id_contafb", "" },
                    { "ps", "true" },
                    { "ar", "true" },
                    { "pr", "true" },
                    { "idp", "" },
                    { "idep", "" },
                    { "vsys", "2" }
                }
           );

            //Task_Payload payload = new Task_Payload
            //{
            //    id_contaig = 15799597,
            //    id_contayt = null,
            //    id_contakwai = null,
            //    id_contatt = null,
            //    id_contafb = null,
            //    ps = true,
            //    ar = true,
            //    pr = true,
            //    idp = null,
            //    idep = null,
            //    vsys = 2
            //};

            try
            {
                // Get tasks instagram
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://www.ganharnoinsta.com/json/sistema"),
                    Headers = {
                    { HttpRequestHeader.AcceptEncoding.ToString(), "gzip, deflate, br" },
                    { HttpRequestHeader.AcceptLanguage.ToString(), "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7" },
                    { HttpRequestHeader.ContentLength.ToString(), "111" },
                    { HttpRequestHeader.ContentType.ToString(), "application/x-www-form-urlencoded; charset=UTF-8" },
                    { HttpRequestHeader.Cookie.ToString(), "PHPSESSID=ehsehmte8018tsjjudq3lh84hq; _ga=GA1.2.1935576342.1657001921; _gid=GA1.2.91753242.1657001921; _gac_UA-90889597-12=1.1657065141.CjwKCAjwwo-WBhAMEiwAV4dybapkcrf_opVFQKslTosI6r_mtoflAOaYZsqThYWtuZJU4SERaukxtxoCtUEQAvD_BwE" },
                    { HttpRequestHeader.Referer.ToString(), "https://www.ganharnoinsta.com/painel/?pagina=sistema" },
                    { HttpRequestHeader.UserAgent.ToString(), "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.0.0 Safari/537.36" },
                    { "path", "/json/sistema" },
                    { "origin", "https://www.ganharnoinsta.com" },
                    { "sec-ch-ua", ".Not/A)Brand;v=99, Google Chrome;v=103, Chromium;v=103" },
                    { "sec-ch-ua-mobile", "?0" },
                    { "sec-ch-ua-platform", "Windows" },
                    { "sec-fetch-dest", "empty" },
                    { "sec-fetch-mode", "cors" },
                    { "sec-fetch-site", "same-origin" },
                    { "user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.0.0 Safari/537.36" },
                    { "x-requested-with", "XMLHttpRequest" },

                },
                    Content = payload
                };

                var response = await client.SendAsync(httpRequestMessage).Result.Content.ReadAsStringAsync();

                Task_Received reponseFormatted = JsonConvert.DeserializeObject<Task_Received>(response);
                
                if (reponseFormatted.status == "ok")
                {
                    Console.Out.WriteLine(reponseFormatted);
                }
                else
                {
                    Console.Out.WriteLine("Nenhuma tarefa encontrada, rodando novamente...");
                }
            }
            catch (Exception error)
            {

                throw error;
            }
        }
    }
}
