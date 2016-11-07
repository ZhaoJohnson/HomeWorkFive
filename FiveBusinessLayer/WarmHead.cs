using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiveCommonLayer;
using FiveDataLayer.DAO;
using FiveDataLayer.DbModel;
using FiveModel;
using FiveModel.WebModel;

namespace FiveBusinessLayer
{
    public class WarmHead
    {
        private static StockMarketDao Dao = new StockMarketDao();


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
            var data= GetEmDataModelByUrl(Htmlsb.ToString());
            
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
            stockReport.Companycode = emDataDetailModel.companyCode;
            stockReport.ReportTime = emDataDetailModel.datetime;
            stockReport.Infocode = emDataDetailModel.infoCode;
            stockReport.InsCode = emDataDetailModel.insCode;
            stockReport.InsName = emDataDetailModel.insName;
            stockReport.InsStar = emDataDetailModel.insStar;
            stockReport.Rate = emDataDetailModel.rate;
            stockReport.StockCodeId = stock.StockCodeId;
            stockReport.Title = emDataDetailModel.title;
            stockReport.SratingName = emDataDetailModel.sratingName;
            stockReport.NewPrice = emDataDetailModel.newPrice;
            stockReport.FutureProfit = emDataDetailModel.sy;
            Institution institution = new Institution()
            {
                CreatedAt = DateTimeOffset.Now,
                LastModifiedAt = DateTimeOffset.Now,
                InstitutionCode = emDataDetailModel.insCode,
                InstitutionName = emDataDetailModel.insName,
                InstitutionStar = emDataDetailModel.insStar
            };
            SaveEmData(stock, stockReport, institution);
        }

        private void SaveEmData(Stock stock, StockReport stockReport, Institution institution)
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
                    AddNewStockData(stock, stockReport, institution);
                    // StockCodes.Add(stock.StockCodeId);
                }
            }
            catch (Exception ex)
            {
                    MyLog.OutputAndSaveTxt(ex.Message);
            }
           
        }

        private void AddNewStockData(Stock stock, StockReport stockReport, Institution institution)
        {
            Dao.StockReportModelDao.Add(stockReport);
        }

        private string ReBuildData(string html)
        {
            return html.Substring(html.IndexOf('=') + 1);
        }
    }
}
