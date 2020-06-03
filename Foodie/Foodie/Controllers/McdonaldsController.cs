using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using AngleSharp.Html.Parser;//nuget套件//用AngleSharp套件來找Html的DOM元素以取得特定內容

namespace Foodie.Controllers
{
    public class McdonaldsController : Controller
    {
        // GET: Mcdonalds
        public async System.Threading.Tasks.Task<ActionResult> Mcdonalds()
        {
            string str1 = await GetHtmlAsync();//全部文本 //await: 不會封鎖非同步方法的執行緒B，若A要跑很久可以同時跑B，等A執行完畢，再取得並回傳執行結果。
            string str2 = await AnalyzeHtml(str1);//從全部文本分析出我要的
            ViewBag.strAll = str2;//用ViewBag接我要丟到View的資料

            return View();
        }

        //用HttpClient撈。對網址發送請求，把HTML整個文本撈下來
        private async System.Threading.Tasks.Task<string> GetHtmlAsync()
        {
            HttpClient httpClient = new HttpClient();//HttpClient:發送Http請求到遠端伺服器去取得資料的一種class

            string url = "https://yoti.life/mcdonalds-menu/";//網址
            var responseMessage = await httpClient.GetAsync(url);//發送請求並取得回應內容

            //檢查回應的伺服器狀態StatusCode是否是200 OK
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string responseResult = responseMessage.Content.ReadAsStringAsync().Result; //取得Content內容(該網頁的HTML格式資料)
                
                return responseResult;
            }
            return "";
        }

        //用using AngleSharp.Html.Parser;方式解析。
        private async System.Threading.Tasks.Task<string> AnalyzeHtml(string html)
        {
            string str = "";
            var parser = new HtmlParser();
            var document = await parser.ParseDocumentAsync(html);
            
            //抓標籤
            var contents = document.QuerySelectorAll("tr"); //QuerySelectorAll("tr"):找出class="tr"所有元素

            foreach (var c in contents)
            {
                str += c.TextContent;
                //str = str + "\r\n";
                
            }
            return str;
        }
    }
}