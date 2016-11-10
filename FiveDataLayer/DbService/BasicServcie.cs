using FiveDataLayer.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveDataLayer.DbService
{
    public abstract class BasicServcie<Tclass> : StockEntityFramework<Entities>
        where Tclass:class
    {
        public virtual Tclass Add(Tclass t)
        {
            return ExecEntitySqlOnOperationData(ef => ef.Set<Tclass>().Add(t), true);
        }
    }
}