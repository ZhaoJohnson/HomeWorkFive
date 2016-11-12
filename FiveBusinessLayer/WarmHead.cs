using FiveCommonLayer;
using FiveDataLayer.DbModel;
using FiveDataLayer.DbService;
using FiveModel;
using FiveModel.WebModel;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FiveBusinessLayer
{
    public class WarmHead
    {
        private static int allpage;
        private static bool isGoOn = true;
        private static object LockForPage = new object();
        private static bool isGoingPage = true;
        private static bool TheadLock = true;
        private const int PageSize = 50;
        private Dictionary<string, Stock> stockcodes = new Dictionary<string, Stock>();
        private ShowService Service = new ShowService();

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
            Service.StockOperationTrackingService.Add(stockoperation);
        }

        public void WarmStar()
        {
            List<Task> taskList = new List<Task>();
            TaskFactory taskFactory = new TaskFactory();
            int datarows = PageSize;
            int page = 1;
            do
            {
                if (!isGoingPage) continue;
                lock (LockForPage)
                {
                    Console.WriteLine($"正在读取第{page}页的数据" + "当前线程：" + Thread.CurrentThread.ManagedThreadId);
                    StringBuilder Htmlsb = new StringBuilder();
                    Htmlsb.Append(
                        "http://datainterface.eastmoney.com//EM_DataCenter/js.aspx?type=SR&sty=GGSR&js=var%20{jsname}=");
                    Htmlsb.Append("{\"data\":[(x)],\"pages\":\"(pc)\",\"update\":\"(ud)\",\"count\":\"(count)\"}");
                    Htmlsb.Append($"&ps={datarows}");
                    Htmlsb.Append($"&p={page}");
                    Htmlsb.Append("&mkt=0&stat=0&cmd=2&code=");

                    //GetEmJson(Htmlsb.ToString());
                    taskList.Add(taskFactory.StartNew(() => GetEmJson(Htmlsb.ToString())));
                    if (taskList.Count > 5)
                    {
                        isGoingPage = false;
                        taskList = taskList.Where(t => !t.IsCompleted && !t.IsCanceled && !t.IsFaulted).ToList();
                        Task.WaitAny(taskList.ToArray());
                        isGoingPage = true;
                    }
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

        public void GetEmJson(string url)
        {
            var data = GetEmDataModelByUrl(url);
            foreach (EmDataDetailModel item in data.data)
            {
                BuildModel(item);
            }
        }

        private EmDataModel GetEmDataModelByUrl(string url)
        {
            var html = HttpHelper.DownloadCommodity(url, "UTF8");
            var datajson = ReBuildData(html);
            EmDataModel data = MyJsonHelper.Json2ObjectByString<EmDataModel>(datajson);
            return data;
        }

        private void BuildModel(EmDataDetailModel emDataDetailModel)
        {
            Stock stock = new Stock();
            stock.IsActivity = true;
            stock.CreatedAt = DateTimeOffset.Now;
            stock.LastModifiedAt = DateTimeOffset.Now;

            #region 分区

            var codes = emDataDetailModel.secuFullCode.Split('.');
            if (string.IsNullOrEmpty(codes[0])) throw new Exception("代码有问题");
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

            #endregion 分区

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
            AddNewStockData(stockReport);
            try
            {
                var StockCodeKey = Service.StockService.GetStockByKey(stock.StockCodeId);
                if (StockCodeKey != null)
                {
                    if (!stockcodes.ContainsKey(stock.StockCodeId))
                        stockcodes.Add(stock.StockCodeId, StockCodeKey);
                    stock.LastModifiedAt = DateTimeOffset.Now;
                    Service.StockService.AddorUpdate(stock);
                    AddNewStockData(stockReport);
                }
                else
                {
                    Service.StockService.Add(stock);
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
            stockReport = Service.StockReportService.Add(stockReport);
            Console.WriteLine($"stockReport插入了一条数据，ID：{stockReport.StockReportId}");
            Console.WriteLine(reportUrl);
            GetReportData(reportUrl, stockReport.StockReportId);
        }

        public void forTest()
        {
            //string url = "http://data.eastmoney.com/report/";
            //AsynWebRequest aw = new AsynWebRequest(url);
            //aw.Navigate();
            Console.ReadKey();
            //string url = "http://data.eastmoney.com/report/20161108/APPH6BZpTVeKASearchReport.html";
            //GetReportData(url);
        }

        private void GetReportData(string url, int reportId)
        {
            var html = HttpHelper.DownloadCommodity(url, "GB2312");
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
                    sb.Append(test);
                }
            }
            Service.StockRepordDataService.Add(new StockRepordData()
            {
                CreatedAt = DateTimeOffset.Now,
                StockReportId = reportId,
                DataReport = sb.ToString()
            });
        }

        private string ReBuildData(string html)
        {
            return html.Substring(html.IndexOf('=') + 1);
        }
    }
}