using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveDataLayer.DAO
{
    public abstract class BasicDao<TDbContext, TPo> : IDisposable
        where TDbContext : DbHelper, new()
        where TPo : class, new()
    {
        protected BasicDao()
        {
            this.DbContext =new TDbContext();
        }
        protected readonly TDbContext DbContext;

        public IEnumerable<TPo> GetData()
        {
            return this.DbContext.GetData<TPo>();
        }

        public virtual bool Add(TPo po)
        {
            return this.DbContext.Add(po);
        }

        public virtual bool Update(TPo updatePo)
        {
            return this.DbContext.Update(updatePo);
        }

        public virtual bool Remove(TPo po)
        {
            return this.DbContext.Delete(po);
        }

        public virtual void Dispose()
        {

        }
    }
}
