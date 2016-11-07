using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiveDataLayer.DbModel;

namespace FiveDataLayer.DAO
{
    public class StockDao : BasicDao<DbHelper, StockModel>
    {
        public StockDao(DbHelper dbContext) : base(dbContext)
        {
        }
    }
}
