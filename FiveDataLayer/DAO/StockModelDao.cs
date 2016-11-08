using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FiveDataLayer.DbModel;

namespace FiveDataLayer.DAO
{
    public class StockModelDao : BasicDao<Entities, Stock>
    {
        private static object lockContext = new object();
        private static bool inSerach = true;
        public StockModelDao(Entities dbContext) : base(dbContext)
        {
        }

        public override IEnumerable<Stock> GetData()
        {
            return this.DbContext.Stock;
        }

        public Stock GetStockBykey(string Key)
        {
           return GetData().FirstOrDefault(p => p.StockCodeId == Key);
        }

        public override Stock Add(Stock po)
        {
            return this.DbContext.Stock.Add(po);
        }

        public override void Update(Stock updatePo)
        {
            this.DbContext.Stock.AddOrUpdate(updatePo);
        }

        public override Stock Remove(Stock po)
        {
            return this.DbContext.Stock.Remove(po);
        }
    }
}
