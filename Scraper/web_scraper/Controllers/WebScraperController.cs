using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using web_scraper.Utils;

namespace web_scraper.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class WebScraperController : ControllerBase
    {
        private readonly ILogger<WebScraperController> _logger;
        public WebScraperController(ILogger<WebScraperController> logger)
        {
            _logger = logger;
        }


        [HttpGet]
        public async Task GetPageConsole(string url = "http://www.rondonia.ro.gov.br/")
        {
            // Carrega a configuração padrão 
            var config = Configuration.Default.WithDefaultLoader();

            // Cria um novo contexto de navegação 
            var context = BrowsingContext.New(config);

            // É aqui que a solicitação HTTP acontece, retorna <IDocument> que // podemos consultar mais tarde             
            var document = await context.OpenAsync(url);

            // Registrar os dados no console 
            _logger.LogInformation(document.DocumentElement.OuterHtml);
        }


        [HttpGet]
        public async Task<dynamic> GetPhones(string url = "http://www.rondonia.ro.gov.br/portal/contato/")
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(url);

            var index = document.DocumentElement.OuterHtml.IndexOf("Telefone");
            var text = document.DocumentElement.OuterHtml.Substring(index, 500);

            string pattern = "\\s+"; // <-- identifica um ou mais caracteres em brancos/vazios            
            Regex rgx = new Regex(pattern);
            string result = rgx.Replace(text, "");

            return result;
        }


        [HttpGet]
        public async Task<dynamic> GetPhones02(string url = "http://www.rondonia.ro.gov.br/portal/contato/")
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(url);

            var index = document.DocumentElement.OuterHtml.IndexOf("Telefone");
            var text = document.DocumentElement.OuterHtml.Substring(index, 500);        

            index = text.IndexOf("69");
            var phone = text.Substring(index, 11);

            return phone;

        }


        [HttpGet]
        public async Task<dynamic> GetPhones03(string url = "http://www.rondonia.ro.gov.br/portal/contato/")
        {
            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(url);

            var index = document.DocumentElement.OuterHtml.IndexOf("Telefone");
            var text = document.DocumentElement.OuterHtml.Substring(index, 500);

            List<string> phones = new List<string>();           

            while (text.Length > 0)
            {
                index = text.IndexOf("69"); // <-- temos a informação de que o DDD é 69

                if (index != -1)
                {
                    var phone = text.Substring(index, 12);
                    phone = StringManipulate.OnlyNumbers(phone);
                    phones.Add(phone);
                    index = index + 11;
                    text = text.Substring(index, text.Length - index);                    
                }
                else
                {
                    break;
                }
            }           

            return phones;
        }
    }
}