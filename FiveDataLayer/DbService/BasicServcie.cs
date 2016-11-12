using FiveDataLayer.DbModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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

        public virtual Tclass QuerySingle(object objectKey)
        {
            return ExecEntitySqlOnOperationData(ef => ef.Set<Tclass>().Find(objectKey));
        }

        public virtual Tclass AddorUpdate(Tclass T)
        {
            return ExecEntitySqlOnOperationData(ef =>
            {
                ef.Set<Tclass>().AddOrUpdate(T);
                return ef.Set<Tclass>().Find(T);
            }, true);
        }
    }
}