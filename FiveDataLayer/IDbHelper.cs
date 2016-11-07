using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveDataLayer
{
    public interface IDbHelper
    {
        IEnumerable<T> GetData<T>() where T : new();
        T GetData<T>(int id) where T : new();
        bool Add<T>(T addModel);
        bool Update<T>(T updateModel);
        bool Delete<T>(T deleteModel);
    }
}
