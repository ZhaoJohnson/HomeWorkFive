using FiveDataLayer.DbService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveDataLayer.DbService
{
    public class ShowService
    {
        public StockService StockService => new StockService();
        public StockReportService StockReportService => new StockReportService();
        public StockRepordDataService StockRepordDataService => new StockRepordDataService();
        public StockOperationTrackingService StockOperationTrackingService => new StockOperationTrackingService();
    }
}