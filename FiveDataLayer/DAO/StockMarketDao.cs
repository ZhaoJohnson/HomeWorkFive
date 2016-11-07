using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveDataLayer.DAO
{
    public class StockMarketDao
    {
        public StockMarketDao(DbHelper dbContext)
        {
            this.DbContext = dbContext;
        }
        protected readonly DbHelper DbContext;
    }
}
