using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace FiveCommonLayer
{
    public class EmailHelper
    {
        public static void SendMailUseZj(string subject, string body)
        {
            System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
            msg.To.Add("63327648@qq.com");
            /*
            * msg.To.Add("b@b.com");
            * msg.To.Add("b@b.com");
            * msg.To.Add("b@b.com");可以发送给多人
            */

            /*
            * msg.CC.Add("c@c.com");
            * msg.CC.Add("c@c.com");可以抄送给多人
            */
            msg.From = new MailAddress("unfair113@126.com", "MyStockHost", System.Text.Encoding.UTF8);
            /* 上面3个参数分别是发件人地址（可以随便写），发件人姓名，编码*/
            msg.Subject = subject;//邮件标题
            msg.SubjectEncoding = System.Text.Encoding.UTF8;//邮件标题编码
            msg.Body = body;//邮件内容
            msg.BodyEncoding = System.Text.Encoding.UTF8;//邮件内容编码
            msg.IsBodyHtml = false;//是否是HTML邮件
            msg.Priority = MailPriority.High;//邮件优先级

            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential("unfair113@126.com", "153486+Aa");
            //在71info.com注册的邮箱和密码
            client.Host = "smtp.126.com";
            object userState = msg;
            try
            {
                //client.SendAsync(msg, userState);
                client.Send(msg);
                //简单一点儿可以client.Send(msg);
                Console.WriteLine("发送成功");
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                Console.WriteLine(ex.Message, "发送邮件出错");
            }
        }
    }
}