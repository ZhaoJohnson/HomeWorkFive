using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiveDataLayer.DbModel;

namespace FiveDataLayer.DAO
{
    public class StockModelDao : BasicDao<DbHelper, StockModel>
    {

        protected DbHelper dbContext;
        public StockModelDao() : base()
        {
            this.dbContext= new DbHelper(ConfigurationManager.ConnectionStrings["StockMarket"].ConnectionString);
        }
    }
}
