using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FiveModel
{
    public class EmDataDetailModel
    {
        /// <summary>
        /// 评委
        /// </summary>
        public string author { get; set; }

        /// <summary>
        /// 评级
        /// </summary>
        public string change { get; set; }

        /// <summary>
        ///
        /// </summary>
        public int companyCode { get; set; }

        /// <summary>
        /// 发布日期
        /// </summary>
        public DateTime datetime { get; set; }

        /// <summary>
        /// report链接地址
        /// </summary>
        public string infoCode { get; set; }

        /// <summary>
        /// 推荐券商ID
        /// </summary>
        public string insCode { get; set; }

        /// <summary>
        /// 推荐券商名称
        /// </summary>
        public string insName { get; set; }

        /// <summary>
        /// 推荐券商级别
        /// </summary>
        public int insStar { get; set; }

        /// <summary>
        /// 没什么用 int.p
        /// </summary>

        //public string[] jlrs { get; set; }

        /// <summary>
        /// 建议
        /// </summary>
        public string rate { get; set; }

        /// <summary>
        /// 股票代码 XXXXXX.SH/SZ
        /// </summary>
        public string secuFullCode { get; set; }

        /// <summary>
        /// 股票名称
        /// </summary>
        public string secuName { get; set; }

        /// <summary>
        /// 建议层度
        /// </summary>
        public string sratingName { get; set; }

        /// <summary>
        /// 没什么用
        /// </summary>
        public decimal sy { get; set; }

        /// <summary>
        /// 市盈率 数组，第一个为本年度，第二个为明年预测  (4,2)
        /// </summary>
        //public decimal[] syls { get; set; }

        /// <summary>
        /// 收益 数组，第一个为本年度，第二个为明年预测  (6,4)
        /// </summary>
        //public decimal[] sys { get; set; }

        /// <summary>
        /// 推荐标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 年报
        /// </summary>
        public int profitYear { get; set; }

        public int type { get; set; }

        /// <summary>
        /// 最新价格
        /// </summary>
        public decimal newPrice { get; set; }
    }
}