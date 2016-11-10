using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FiveDataLayer.DbModel;

namespace FiveDataLayer.DbService
{
    public class StockService : BasicServcie<Stock>
    {
        public Stock GetStockByKey(string key)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId+"当前ID:"+key);
            return ExecEntitySqlOnOperationData(ef => ef.Stock.Find(key));
        }

        public override Stock Update(Stock T)
        {
            return ExecEntitySqlOnOperationData(ef =>
            {
                ef.Set<Stock>().AddOrUpdate(T);
                return ef.Set<Stock>()
                     .FirstOrDefault(p => p.StockCodeId == T.StockCodeId && p.LastModifiedAt == T.LastModifiedAt);

            });
        }
    }
}
