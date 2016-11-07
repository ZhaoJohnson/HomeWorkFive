using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveDataLayer.DbModel
{
    public class StockModel
    {
        public int StockCode { get; set; }
        public string StockName { get; set; }
        public int StockTypeId { get; set; }
        public bool IsActivity { get; set; }
        public Nullable<System.DateTimeOffset> CreatedAt { get; set; }
        public Nullable<System.DateTimeOffset> LastModifiedAt { get; set; }
    }
}

