using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace ToolBox.Logging {

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
            var sender = App.SenderEmailAddress;
            var receipient = App.InfoEmailAddress;
            Send(sender, receipient, subject, body);            
        }

        public static void Alert(string subject, string body)  {
            var sender = App.SenderEmailAddress;
            var receipient = App.AlertEmailAddress;
            Send(sender, receipient, subject, body); 
        }
    }
}
