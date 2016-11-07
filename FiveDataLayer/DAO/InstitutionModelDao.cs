using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiveDataLayer.DbModel;

namespace FiveDataLayer.DAO
{
    public class InstitutionModelDao : BasicDao<DbHelper, Institution>
    {
        public InstitutionModelDao(DbHelper dbContext) : base(dbContext)
        {
        }
    }
}
