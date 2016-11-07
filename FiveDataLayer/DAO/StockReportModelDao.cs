using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiveDataLayer.DbModel;

namespace FiveDataLayer.DAO
{
    public class StockReportModelDao: BasicDao<DbHelper,StockReportModel>
    {
        public StockReportModelDao(DbHelper dbContext) : base(dbContext)
        {
        }
    }
}
