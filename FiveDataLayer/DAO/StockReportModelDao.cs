using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiveDataLayer.DbModel;

namespace FiveDataLayer.DAO
{
    public class StockReportModelDao : BasicDao<Entities, StockReport>
    {
        public StockReportModelDao(Entities dbContext) : base(dbContext)
        {
        }

        public override IEnumerable<StockReport> GetData()
        {
            return this.DbContext.StockReport;
        }

        public override StockReport Add(StockReport po)
        {
            return this.DbContext.StockReport.Add(po);
        }

        public override void Update(StockReport updatePo)
        {
            this.DbContext.StockReport.AddOrUpdate(updatePo);
        }

        public override StockReport Remove(StockReport po)
        {
            var result= this.DbContext.StockReport.Remove(po);
            return result;
        }
    }

}

       