using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiveDataLayer.AttributeModel;

namespace FiveDataLayer.DbModel
{
    public class StockType
    {
        [PrimaryKey(false)]
        public int StockTypeId { get; set; }
        public string StockTypeName { get; set; }
        public string description { get; set; }
        public Nullable<System.DateTimeOffset> CreatedAt { get; set; }
    }
}
