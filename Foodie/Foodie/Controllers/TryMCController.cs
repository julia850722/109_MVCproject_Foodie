using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace Foodie.Controllers
{
    public class TryMCController : Controller
    {
        // GET: Mcdonalds
        public async System.Threading.Tasks.Task<ActionResult> TryMC()//async System.Threading.Tasks.Task<ActionResult>: 抓別的網站時事另外開一個執行緒B去跑
        {
            string str = await GetHtmlAsync();//全部文本 //await: 不會封鎖非同步方法的執行緒B，若A要跑很久可以同時跑B，等A執行完畢，再取得並回傳執行結果。
            string str2 = await AnalyzeHtml(str);//分析出我要的
            ViewBag.StrAll = str2;//用Str接我要丟到View的資料

            return View();
        }

        //用HttpClient撈。對網址發送請求，把HTML整個文本撈下來
        private async System.Threading.Tasks.Task<string> GetHtmlAsync()
        {
            HttpClient httpClient = new HttpClient();

            string url = "https://yoti.life/mcdonalds-menu/";
            var responseMessage = await httpClient.GetAsync(url); //發送請求

            //檢查回應的伺服器狀態StatusCode是否是200 OK
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string responseResult = responseMessage.Content.ReadAsStringAsync().Result; //取得內容

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
            var contents = document.QuerySelectorAll("tr");//把tr標籤內全部抓出

            foreach (var c in contents)
            {
                var str_innerHtml = c.TextContent;//只取HTML文字部分
                var price = str_innerHtml.Contains("$"); //回傳是True/False
                
                if (str_innerHtml.Contains("$"))//是否包含價錢
                {
                    str += c.TextContent;
                    price = false;
                }
                str += c.TextContent;

            }
            return str;
        }
    }
}