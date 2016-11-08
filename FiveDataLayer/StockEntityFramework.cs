using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using FiveDataLayer.DbModel;

namespace FiveDataLayer
{
    public abstract class StockEntityFramework<TDbContext>
        where TDbContext : DbContext, new()
    {

        protected TResult ExecEntitySqlOnOperationData<TResult>(Func<Entities, TResult> operationDataEntitiesFn)
        {
            return ExecEntitySql(operationDataEntitiesFn);
        }
        protected void ExecEntitySqlOnOperationData(Action<Entities> operationDataEntitiesFn)
        {
            ExecEntitySql(operationDataEntitiesFn);
        }
        //keep transation in Entities
        private TResult ExecEntitySql<TDbContext, TResult>(Func<TDbContext, TResult> dbContextFn, bool needTransaction = false)
            where TDbContext : DbContext, new()
        {
            using (var entities = new TDbContext())
            {
                if (!needTransaction)
                    return dbContextFn(entities);
                using (var scope = new TransactionScope())
                {
                    try
                    {
                        var result = dbContextFn(entities);
                        entities.SaveChanges();
                        scope.Complete();
                        return result;
                    }
                    catch (Exception ex)
                    {
                        //TODO:Save log.
                        throw ex;
                    }
                }
            }
        }
        private void ExecEntitySql<TDbContext>(Action<TDbContext> dbContextFn, bool needTransaction = false)
            where TDbContext : DbContext, new()
        {
            ExecEntitySql<TDbContext, bool>(context => { dbContextFn(context); return true; }, needTransaction);
        }
    }
}
