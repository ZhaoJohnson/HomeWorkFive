using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveDataLayer.DAO
{
    public class StockMarketDao
    {
        public StockMarketDao()
        {
            this.DbContext = new DbHelper(ConfigurationManager.ConnectionStrings["StockMarket"].ConnectionString);
        }
        protected readonly DbHelper DbContext;
        public StockModelDao StockModelDao { get; set; }
        public StockTypeDao StockTypeDao { get; set; }
        public StockReportModelDao StockReportModelDao { get; set; }
        public InstitutionModelDao InstitutionModelDao { get; set; }
    }
}
