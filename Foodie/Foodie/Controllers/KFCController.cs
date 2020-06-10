using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AngleSharp.Html.Parser;

namespace Foodie.Controllers
{
    public class KFCController : Controller
    {
        // GET: KFC
        //public ActionResult Index()
        //{
        //    return View();
        //}
        public async System.Threading.Tasks.Task<ActionResult> KFCAllMeal()
        {
            string str = await GetHtmlAsync();
            string str2 = await AnalyzeHtml(str);
            ViewBag.Str3 = str2;

            return View();
        }

        private async System.Threading.Tasks.Task<string> GetHtmlAsync()
        {
            HttpClient httpClient = new HttpClient();

            string url = "https://yoti.life/kfc-menu/";
            var responseMessage = await httpClient.GetAsync(url); //發送請求

            //檢查回應的伺服器狀態StatusCode是否是200 OK
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string responseResult = responseMessage.Content.ReadAsStringAsync().Result; //取得內容

                return responseResult;
            }
            return "";
        }//name-desc
        private async System.Threading.Tasks.Task<string> AnalyzeHtml(string html)
        {
            bool flag = true;
            string temp_td = "";//抓到的一個值
            string temp_tr = "";//一整行
            string str = "";
            string strHtml = "";
            int v_cnt_tr = 0;//第幾行
            int v_cnt_table = 0;//第幾個table
            ArrayList table = new ArrayList();
            ArrayList tr = new ArrayList();
            ArrayList td = new ArrayList();

            //需disable掉的table編號
            int[] disable_table = new int[] { 0, 1, 4, 7, 8, 9, 10, 13, 14, 15, 16, 23, 24, 25, 26};

            var parser = new HtmlParser();
            var document = await parser.ParseDocumentAsync(html);
            //var contents = document.QuerySelectorAll("tr > td").Select(x => x.NextElementSibling);
            var contents = document.QuerySelectorAll("*").Where(x => new[] { "table", "tr", "td" }.Contains(x.TagName.ToLower()));

            foreach (var c in contents)
            {
                if (c.TagName == "TABLE")
                    table.Add(c.TextContent.ToString().Replace("\n", "").Replace(" ", "").Replace("\t", ""));
                if (c.TagName == "TR")
                    tr.Add(c.TextContent.ToString().Replace("\n", "").Replace(" ", "").Replace("\t", ""));
                if (c.TagName == "TD")
                    td.Add(c.TextContent.ToString().Replace("\n", "").Replace(" ", "").Replace("\t", ""));
            }

            for (int i = 0; i < td.Count; i++)
            {
                flag = true;
                temp_td += td[i];
                str += "<td>" + td[i] + "</td>";

                if (v_cnt_tr < tr.Count && temp_td == tr[v_cnt_tr].ToString())//若抓完一整行會進入
                {
                    temp_tr += temp_td;//一整行(不含分行符號) => 比對用
                    temp_td = "";
                    str = "<tr>" + str + "</tr>";//加橫線*2 //一整行(含分行符號)
                    v_cnt_tr++;//往下一行前進

                    if (v_cnt_tr > 0)
                    {
                        if (v_cnt_table < table.Count && temp_tr == table[v_cnt_table].ToString())//若抓到完整table會進入
                        {
                            for (int j = 0; j < disable_table.Length; j++)//篩掉不要的table
                            {
                                if (v_cnt_table == disable_table[j])
                                {
                                    if (v_cnt_table == 4)
                                    {
                                        strHtml = strHtml + " <h3> 重量級XL套餐 </h3> " + "<br>";
                                    }
                                    else if (v_cnt_table == 10)
                                    {
                                        strHtml = strHtml + " <h3> 任你選全家餐 </h3> " + "<br>";
                                    }
                                    else if (v_cnt_table == 16)
                                    {
                                        strHtml = strHtml + " <h3> 單點品項 </h3> " + "<br>";
                                    }

                                    flag = false;
                                    break;
                                }
                            }

                            if (flag)//要的table
                            {
                                
                                str = "<table style=\"font-size:20px; font-family: Lucida Console; background-color: #FFE8BF; border: 5px #FF6363;\" cellpadding=\"10\" border=\"1\" >" + str + "</table>";
                                strHtml += str + "\n" + "<br>" + "<br>" + "<br>" + "\n";
                                
                                if(v_cnt_table == 2)//加標題
                                {
                                    strHtml = " <h3> 個人獨享餐 </h3> " + "<br>" + strHtml;
                                }
                                //else if (v_cnt_table == 4)
                                //{
                                //    strHtml = strHtml + " <h3> 重量級XL套餐 </h3> " + "<br>";
                                //}
                                //else if (v_cnt_table == 10)
                                //{
                                //    strHtml = strHtml + " <h3> 任你選全家餐 </h3> " + "<br>";
                                //}
                                else if (v_cnt_table == 11)
                                {
                                    strHtml = strHtml + " <h3> 歡聚餐 </h3> " + "<br>";
                                }
                                //else if (v_cnt_table == 16)
                                //{
                                //    strHtml = strHtml + " <h3> 單點品項 </h3> " + "<br>";
                                //}
                                else if (v_cnt_table == 19)
                                {
                                    strHtml = strHtml + " <h3> 飲料 </h3> " + "<br>";
                                }
                                str = "";
                                temp_td = "";
                                temp_tr = "";
                                v_cnt_table++;

                            }
                            else
                            {
                                str = "";
                                temp_td = "";
                                temp_tr = "";
                                v_cnt_table++;
                            }
                        }
                    }
                }
            }

            return strHtml;
        }
    }
}