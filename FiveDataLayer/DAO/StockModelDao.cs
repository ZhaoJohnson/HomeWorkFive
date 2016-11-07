using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiveDataLayer.DbModel;

namespace FiveDataLayer.DAO
{
    public class StockModelDao : BasicDao<DbHelper, Stock>
    {

        public Stock GetStockCodeByCode(string codeId)
        {
           return this.DbContext.GetData<Stock>(codeId);
        }

        public StockModelDao(DbHelper dbContext) : base(dbContext)
        {
        }
    }
}
