using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace Logging {

    public static class Email {

        private static void Send(string sender, string receipient, 
                                 string subject,string body)  {
          string title = subject;
          var newMailClient = new System.Net.Mail.SmtpClient { Host = "osamsmtp" };
          System.Net.Mail.MailMessage msg2;    // 
          msg2 = new System.Net.Mail.MailMessage(sender,receipient, title, body);
          newMailClient.Send(msg2);
        }

        public static void Inform(string subject, string body)  {
            var sender = Config.Global.SenderEmailAddress;
            var receipient = Config.Global.InfoEmailAddress;
            Send(sender, receipient, subject, body);            
        }

        public static void Alert(string subject, string body)  {
            var sender = Config.Global.SenderEmailAddress;
            var receipient = Config.Global.AlertEmailAddress;
            Send(sender, receipient, subject, body); 
        }


        public static void Send2()
        {
            string title = "Test Title";
            var newMailClient = new System.Net.Mail.SmtpClient { Host = "osamsmtp" };
            
            var inputbody = "\r\n";
            inputbody += " This is a test email.\r\n";
            inputbody += " Don\'t take seriously.\r\n";
            inputbody += " Except if you are\r\n";
            inputbody += " Testing \r\n";

            System.Net.Mail.MailMessage msg2;    // 
            msg2 = new System.Net.Mail.MailMessage("mhernandez@osam.com", "software_it@osam.com", title, inputbody);
            newMailClient.Send(msg2);
        }

    }
}
