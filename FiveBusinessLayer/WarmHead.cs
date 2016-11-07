using FiveCommonLayer;
using FiveDataLayer.DAO;
using FiveDataLayer.DbModel;
using FiveModel;
using FiveModel.WebModel;
using System;
using System.Collections.Generic;
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
            double pages = (data.Count / 20);
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
            int datarows = 50;
            int page = 1;
            do
            {
                lock (LockForPage)
                {
                    StringBuilder Htmlsb = new StringBuilder();
                    Htmlsb.Append(
                        "http://datainterface.eastmoney.com//EM_DataCenter/js.aspx?type=SR&sty=GGSR&js=var%20{jsname}=");
                    Htmlsb.Append("{\"data\":[(x)],\"pages\":\"(pc)\",\"update\":\"(ud)\",\"count\":\"(count)\"}");
                    Htmlsb.Append($"&ps={datarows}");
                    Htmlsb.Append($"&p={page}");
                    Htmlsb.Append("&mkt=0&stat=0&cmd=2&code=");
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
                        Task.WaitAll(taskList.ToArray());
                        TheadLock = false;
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
            //stockReport.FutureProfit = emDataDetailModel.sy;

            SaveEmData(stock, stockReport);
        }

        private void SaveEmData(Stock stock, StockReport stockReport)
        {
            try
            {
                var StockCodeKey = Dao.StockModelDao.GetStockCodeByCode(stock.StockCodeId);
                if (StockCodeKey != null)
                {
                    // StockCodes.Add(StockCodeKey.StockCodeId);
                }
                else
                {
                    Dao.StockModelDao.Add(stock);
                    AddNewStockData(stock, stockReport);
                    // StockCodes.Add(stock.StockCodeId);
                }
            }
            catch (Exception ex)
            {
                MyLog.OutputAndSaveTxt(ex.Message);
            }
        }

        private void AddNewStockData(Stock stock, StockReport stockReport)
        {
            Dao.StockReportModelDao.Add(stockReport);
        }

        private string ReBuildData(string html)
        {
            return html.Substring(html.IndexOf('=') + 1);
        }
    }
}