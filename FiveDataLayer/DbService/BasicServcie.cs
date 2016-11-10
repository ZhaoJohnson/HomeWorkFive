using FiveDataLayer.DbModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FiveDataLayer.DbService
{
    public abstract class BasicServcie<Tclass> : StockEntityFramework<Entities>
        where Tclass:class
    {
        public virtual Tclass Add(Tclass t)
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId + "当前ID:" + typeof(Tclass).Name);
            return ExecEntitySqlOnOperationData(ef => ef.Set<Tclass>().Add(t), true);
        }

        public abstract Tclass Update(Tclass T);
    }
}