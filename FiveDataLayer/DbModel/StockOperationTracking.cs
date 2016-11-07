using FiveDataLayer.AttributeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveDataLayer.DbModel
{
    public class StockOperationTracking
    {
        [PrimaryKey(true)]
        public int StockOperationTrackingId { get; set; }

        public DateTime OperationDate { get; set; }
        public int? StockCount { get; set; }
        public DateTimeOffset? CreatedAt { get; set; }
    }
}