using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using FiveDataLayer.DbModel;

namespace FiveDataLayer.DAO
{
    public abstract class BasicDao<TDbContext, TPo> : IBasicDao,IDisposable
        where TDbContext : Entities, new()
        where TPo : class, new()
    {
        protected BasicDao(TDbContext dbContext)
        {
            this.DbContext = dbContext;
        }
        protected readonly TDbContext DbContext;

        public abstract IEnumerable<TPo> GetData();

        public abstract TPo Add(TPo po);

        public abstract void Update(TPo updatePo);

        public abstract TPo Remove(TPo po);

        public void SaveChanges()
        {
            this.DbContext.SaveChanges();
        }

        public virtual void Dispose()
        {

        }

    }
}
