using FiveDataLayer.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveDataLayer.DbService
{
    public class MainServcie : StockEntityFramework<Entities>
    {
        public virtual T Add<T>(T t)
            where T : class
        {
            return ExecEntitySqlOnOperationData(ef =>
            {
                var db = ef.Set<T>();
                var restule = db.Add(t);
                //ef.SaveChanges();
                return restule;
            }, true);
        }
    }
}