using FiveCommonLayer;
using FiveDataLayer.DAO;
using FiveDataLayer.DbModel;
using FiveDataLayer.DbService;
using FiveModel;
using FiveModel.WebModel;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveBusinessLayer
{
    public class WarmHead
    {
        private static StockMarketDao Dao = new StockMarketDao();
        private static int allpage;
        private static bool isGoOn = true;
        private static object LockForPage = new object();
        private static bool isGoingPage = true;
        private static bool TheadLock = true;
        private const int PageSize = 50;
        private Dictionary<string, Stock> stockcodes = new Dictionary<string, Stock>();

        public void FirstOfAll()
        {
            int datarows = 1;
            int page = 1;
            StringBuilder Htmlsb = new StringBuilder();
            Htmlsb.Append(
                "http://datainterface.eastmoney.com//EM_DataCenter/js.aspx?type=SR&sty=GGSR&js=var%20{jsname}=");
            Htmlsb.Append("{\"data\":[(x)],\"pages\":\"(pc)\",\"update\":\"(ud)\",\"count\":\"(count)\"}");
            Htmlsb.Append($"&ps={datarows}");
            Htmlsb.Append($"&p={page}");
            Htmlsb.Append("&mkt=0&stat=0&cmd=2&code=");
            var data = GetEmDataModelByUrl(Htmlsb.ToString());
            double pages = (data.Count / PageSize);
            allpage = int.Parse(Math.Ceiling(pages).ToString());
            Console.WriteLine($"共计：{allpage}页");
            StockOperationTracking stockoperation = new StockOperationTracking()
            {
                StockCount = data.Count,
                OperationDate = data.update,
                CreatedAt = DateTimeOffset.Now
            };
            Dao.StockOperationTrackingDao.Add(stockoperation);
        }

        public void WarmStar()
        {
            List<Task> taskList = new List<Task>();
            TaskFactory taskFactory = new TaskFactory();
            int datarows = PageSize;
            int page = 1;
            do
            {
                lock (LockForPage)
                {
                    Console.WriteLine($"正在读取第{page}页的数据");
                    StringBuilder Htmlsb = new StringBuilder();
                    Htmlsb.Append(
                        "http://datainterface.eastmoney.com//EM_DataCenter/js.aspx?type=SR&sty=GGSR&js=var%20{jsname}=");
                    Htmlsb.Append("{\"data\":[(x)],\"pages\":\"(pc)\",\"update\":\"(ud)\",\"count\":\"(count)\"}");
                    Htmlsb.Append($"&ps={datarows}");
                    Htmlsb.Append($"&p={page}");
                    Htmlsb.Append("&mkt=0&stat=0&cmd=2&code=");
                    GetEmJson(Htmlsb.ToString());
                    //taskList.Add(taskFactory.StartNew(() => GetEmJson(Htmlsb.ToString())));
                    //if (taskList.Count > 5)
                    //{
                    //    isGoingPage = false;
                    //    taskList = taskList.Where(t => !t.IsCompleted && !t.IsCanceled && !t.IsFaulted).ToList();
                    //    //Task.WaitAny(taskList.ToArray());
                    //    isGoingPage = true;
                    //}
                    if (page < allpage)
                        page++;
                    if (page == allpage)
                    {
                        //Task.WaitAll(taskList.ToArray());
                        TheadLock = false;
                        Console.WriteLine("搞完了");
                        Console.ReadKey();
                    }
                }
            } while (TheadLock);
        }

        //private static Dictionary<string,DateTime> StockCodes =new List<string>();
        public void GetEmJson(string url)
        {
            var data = GetEmDataModelByUrl(url);
            //List<EmDataDetailModel> listDetailModels = new List<EmDataDetailModel>();
            foreach (EmDataDetailModel item in data.data)
            {
                buildModel(item);
            }
        }

        private EmDataModel GetEmDataModelByUrl(string url)
        {
            var html = HttpHelper.DownloadCommodity(url);
            var datajson = ReBuildData(html);
            EmDataModel data = MyJsonHelper.Json2ObjectByString<EmDataModel>(datajson);
            return data;
        }

        private void buildModel(EmDataDetailModel emDataDetailModel)
        {
            Stock stock = new Stock();
            stock.IsActivity = true;
            stock.CreatedAt = DateTimeOffset.Now;
            stock.LastModifiedAt = DateTimeOffset.Now;
            var codes = emDataDetailModel.secuFullCode.Split('.');
            //int stockcode;
            if (string.IsNullOrEmpty(codes[0])) throw new Exception("代码有问题");
            //int.TryParse(codes[0].ToString(), out stockcode);
            stock.StockCodeId = codes[0];
            if (string.IsNullOrEmpty(codes[1])) throw new Exception("代码有问题");
            switch (codes[1])
            {
                case "SZ":
                    stock.StockTypeId = 2;
                    break;

                case "SH":
                    stock.StockTypeId = 1;
                    break;

                default:
                    throw new Exception("怎么可能");
            }
            stock.StockName = emDataDetailModel.secuName;

            StockReport stockReport = new StockReport();
            stockReport.Author = emDataDetailModel.author;
            stockReport.Change = emDataDetailModel.change;
            stockReport.Companycode = emDataDetailModel.companyCode ?? null;
            stockReport.ReportTime = emDataDetailModel.datetime;
            stockReport.Infocode = emDataDetailModel.infoCode;
            stockReport.InsCode = emDataDetailModel.insCode;
            stockReport.InsName = emDataDetailModel.insName;
            stockReport.InsStar = emDataDetailModel.insStar ?? null;
            stockReport.Rate = emDataDetailModel.rate;
            stockReport.StockCodeId = stock.StockCodeId;
            stockReport.Title = emDataDetailModel.title;
            stockReport.SratingName = emDataDetailModel.sratingName;
            stockReport.NewPrice = emDataDetailModel.newPrice ?? null;
            SaveEmData(stock, stockReport);
        }

        private void SaveEmData(Stock stock, StockReport stockReport)
        {
            try
            {
                var StockCodeKey = Dao.StockModelDao.GetStockBykey(stock.StockCodeId);
                if (StockCodeKey != null)
                {
                    if (!stockcodes.ContainsKey(stock.StockCodeId))
                        stockcodes.Add(stock.StockCodeId, StockCodeKey);
                    stock.LastModifiedAt = DateTimeOffset.Now;
                    Dao.StockModelDao.Update(stock);
                    AddNewStockData(stockReport);
                }
                else
                {
                    Dao.StockModelDao.Add(stock);
                    Dao.StockModelDao.SaveChanges();
                    AddNewStockData(stockReport);
                }
            }
            catch (Exception ex)
            {
                MyLog.OutputAndSaveTxt(ex.Message);
            }
        }

        private void AddNewStockData(StockReport stockReport)
        {
            var reportUrl = string.Format("http://data.eastmoney.com/report/{0}/{1}.html", stockReport.ReportTime.Value.ToString("yyyyMMdd"), stockReport.Infocode);
            stockReport.DataReportUrl = reportUrl;
            Dao.StockReportModelDao.Add(stockReport);
            Dao.StockReportModelDao.SaveChanges();
            Console.WriteLine($"插入了一条数据，ID：{stockReport.StockReportId}");
        }

        public void forTest()
        {
            MainServcie stservice = new MainServcie();
            stservice.Add(new StockType()
            {
                CreatedAt = DateTime.Now,
                description = "test",
                StockTypeId = 5,
                StockTypeName = "test"
            });
            Console.ReadKey();
            //string url = "http://data.eastmoney.com/report/20161108/APPH6BZpTVeKASearchReport.html";
            //GetReportData(url);
        }

        private void GetReportData(string url)
        {
            var html = HttpHelper.DownloadCommodity(url);
            HtmlDocument htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(html);
            string innerData = "//*[@id=\"ContentBody\"]/div/p";
            var nodes = htmldoc.DocumentNode.SelectNodes(innerData);
            StringBuilder sb = new StringBuilder();
            if (nodes != null)
            {
                for (int i = 1; i < nodes.Count; i++)
                {
                    var test = nodes[i].InnerText;
                    if (string.IsNullOrEmpty(test)) continue;
                    //乱码不知道怎么解决了。。。。
                    // var s = System.Text.Encoding.GetEncoding("UTF-8").GetString(System.Text.Encoding.UTF8.GetBytes(test));
                    sb.Append(test);
                    MyLog.OutputAndSaveTxt(test);
                }
            }
            Console.WriteLine(sb.ToString());
        }

        private string ReBuildData(string html)
        {
            return html.Substring(html.IndexOf('=') + 1);
        }
    }
}