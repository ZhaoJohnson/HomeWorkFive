using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveModel.WebModel
{
    public class EmDataModel
    {
        public EmDataDetailModel[] data { get; set; }
        public int pages { get; set; }
        public DateTime update { get; set; }
        public int Count { get; set; }
    }
}
