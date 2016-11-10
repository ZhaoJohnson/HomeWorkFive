using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiveDataLayer.DbModel;

namespace FiveDataLayer.DbService
{
    public class StockService:BasicServcie<Stock>
    {
        public Stock GetStockByKey(string key)
        {
            return ExecEntitySqlOnOperationData(ef =>
            {
                var restule = ef.Set<Stock>().First(p=>p.StockCodeId==key);
                return restule;
            });
        }
    }
}
