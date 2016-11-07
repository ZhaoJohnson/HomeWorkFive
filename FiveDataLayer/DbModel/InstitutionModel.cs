using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveDataLayer.DbModel
{
    public class InstitutionModel
    {
        public int InstitutionId { get; set; }
        public Nullable<int> InstitutionCode { get; set; }
        public string InstitutionName { get; set; }
        public Nullable<int> InstitutionStar { get; set; }
        public Nullable<System.DateTimeOffset> CreatedAt { get; set; }
        public Nullable<System.DateTimeOffset> LastModifiedAt { get; set; }
    }
}
