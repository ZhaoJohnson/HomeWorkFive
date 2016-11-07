using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
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
        public StockModelDao StockModelDao => new StockModelDao(this.DbContext);
        public StockTypeDao StockTypeDao => new StockTypeDao(this.DbContext);
        public StockReportModelDao StockReportModelDao => new StockReportModelDao(this.DbContext);
        public InstitutionModelDao InstitutionModelDao =>new InstitutionModelDao(this.DbContext);
    }
}
