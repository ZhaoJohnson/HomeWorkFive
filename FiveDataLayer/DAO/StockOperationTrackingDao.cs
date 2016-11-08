using FiveDataLayer.DbModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveDataLayer.DAO
{
    public class StockOperationTrackingDao : BasicDao<Entities, StockOperationTracking>
    {
        public StockOperationTrackingDao(Entities dbContext) : base(dbContext)
        {
        }

        public override IEnumerable<StockOperationTracking> GetData()
        {
            return this.DbContext.StockOperationTracking;
        }

        public override StockOperationTracking Add(StockOperationTracking po)
        {
            return this.DbContext.StockOperationTracking.Add(po);
        }

        public override void  Update(StockOperationTracking updatePo)
        {
            this.DbContext.StockOperationTracking.AddOrUpdate(updatePo);
        }

        public override StockOperationTracking Remove(StockOperationTracking po)
        {
            return this.DbContext.StockOperationTracking.Remove(po);
        }
    }
}