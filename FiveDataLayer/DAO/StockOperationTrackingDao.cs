using FiveDataLayer.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveDataLayer.DAO
{
    public class StockOperationTrackingDao : BasicDao<DbHelper, StockOperationTracking>
    {
        public StockOperationTrackingDao(DbHelper dbContext) : base(dbContext)
        {
        }
    }
}