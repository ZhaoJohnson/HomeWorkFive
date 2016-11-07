using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FiveBusinessLayer;
using FiveCommonLayer;

namespace HomeWorkFive
{
    class Program
    {
        static void Main(string[] args)
        {
            // http://datainterface.eastmoney.com//EM_DataCenter/js.aspx?type=SR&sty=GGSR&js=var%20{jsname}={"data":[(x)],"pages":"(pc)","update":"(ud)","count":"(count)"}&ps=1&p=1&mkt=0&stat=0&cmd=2&code=

            try
            {
                int datarows = 1;
                int page = 1;
                StringBuilder Htemlsb=new StringBuilder();
                Htemlsb.Append(
                    "http://datainterface.eastmoney.com//EM_DataCenter/js.aspx?type=SR&sty=GGSR&js=var%20{jsname}=");
                Htemlsb.Append("{\"data\":[(x)],\"pages\":\"(pc)\",\"update\":\"(ud)\",\"count\":\"(count)\"}");
                Htemlsb.Append($"&ps={datarows}");
                Htemlsb.Append($"&p={page}");
                Htemlsb.Append("&mkt=0&stat=0&cmd=2&code=");
                WarmHead warmHead=new WarmHead();
                warmHead.GetEmJson(Htemlsb.ToString());
            }
            catch (Exception ex)
            {
                MyLog.OutputAndSaveTxt(ex.Message);
                //throw;
            }

        }
    }
}
