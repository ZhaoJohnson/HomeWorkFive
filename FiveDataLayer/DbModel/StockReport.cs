//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FiveDataLayer.DbModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class StockReport
    {
        public int StockReportId { get; set; }
        public string StockCodeId { get; set; }
        public string Author { get; set; }
        public string Change { get; set; }
        public Nullable<int> Companycode { get; set; }
        public Nullable<System.DateTime> ReportTime { get; set; }
        public Nullable<int> InstitutionId { get; set; }
        public string Infocode { get; set; }
        public string InsCode { get; set; }
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
        public string DataReportUrl { get; set; }
    }
}
