using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiveDataLayer.AttributeModel;

namespace FiveDataLayer.DbModel
{
    public class Stock
    {
        [PrimaryKey(false)]
        public string StockCodeId { get; set; }
        public string StockName { get; set; }
        public int StockTypeId { get; set; }
        public bool IsActivity { get; set; }
        public Nullable<System.DateTimeOffset> CreatedAt { get; set; }
        public Nullable<System.DateTimeOffset> LastModifiedAt { get; set; }
    }
}

