//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FiveDataLayer
{
    using System;
    using System.Collections.Generic;
    
    public partial class StockReport
    {
        public int StockReportId { get; set; }
        public Nullable<int> StockCode { get; set; }
        public string Author { get; set; }
        public string Change { get; set; }
        public Nullable<int> Companycode { get; set; }
        public Nullable<System.DateTime> Datetime { get; set; }
        public Nullable<int> InstitutionId { get; set; }
        public string Infocode { get; set; }
        public Nullable<int> InsCode { get; set; }
        public string InsName { get; set; }
        public Nullable<int> InsStar { get; set; }
        public string Rate { get; set; }
        public string SratingName { get; set; }
        public Nullable<decimal> CurrentProfit { get; set; }
        public Nullable<decimal> FutureProfit { get; set; }
        public Nullable<decimal> CurrentIncomeRate { get; set; }
        public Nullable<decimal> FutureIncomeRate { get; set; }
        public string Title { get; set; }
        public Nullable<int> ProfitYear { get; set; }
        public Nullable<decimal> NewPrice { get; set; }
    }
}
