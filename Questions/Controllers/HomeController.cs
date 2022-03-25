using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Questions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Questions.Controllers
{
    public class HomeController : Controller
    {
       [HttpGet]
        public async Task<ActionResult> Index(int? id)
        {


            HttpClientHandler handler = new HttpClientHandler();
            handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            using (var Client = new HttpClient(handler))
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Client.BaseAddress = new Uri("https://api.stackexchange.com/");
                //HTTP GET
                Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/jason"));
                var response = Client.GetAsync("2.3/questions?order=desc&sort=activity&site=stackoverflow");
                response.Wait();
                var result = response.Result;

                if (result.IsSuccessStatusCode)
                {
                    var Response = result.Content.ReadAsStringAsync().Result;
                  
               
                    StackResponse GetObjects = JsonConvert.DeserializeObject<StackResponse>(Response);
                    var get = GetObjects.items.OrderByDescending(x=>x.creation_date).ToList();
                    if (id != null) { return View("Details",get.SingleOrDefault(x=>x.question_id == id)); }
                    return View(get);
                }

            }

            return View();
        }
       public ActionResult Details() {
            return View();
        }

    }
}