using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiveDataLayer.DbModel;

namespace FiveDataLayer.DAO
{
    public class StockTypeDao : BasicDao<Entities, StockType>
    {
        public StockTypeDao(Entities dbContext) : base(dbContext)
        {
        }

        public override IEnumerable<StockType> GetData()
        {
            return this.DbContext.StockType;
        }

        public override StockType Add(StockType po)
        {
            var result = this.DbContext.StockType.Add(po);
            return result;
        }

        public override void Update(StockType updatePo)
        {
            this.DbContext.StockType.AddOrUpdate(updatePo);
        }


        public override StockType Remove(StockType po)
        {
            var result = this.DbContext.StockType.Remove(po);
            return result;
        }
    }
}
