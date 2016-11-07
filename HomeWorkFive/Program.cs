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
                ActionWarm();
            }
            catch (Exception ex)
            {
                MyLog.OutputAndSaveTxt(ex.Message);
                //throw;
            }

        }

        static void ActionWarm()
        {
            WarmHead warmHead = new WarmHead();
            //warmHead.GetEmJson(ng());
        }
    }
}
