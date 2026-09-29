using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace PenguinAutomatedReporting
{
    internal class Mailer
    {
        SmtpClient Client;
        MailMessage Message;
        public Mailer()
        {
            Client = ConfigureSMTP();
            Message = ConfigureMessage();
        }
        public void SendMail(string xlPath)
        {
            Message.Body = BuildMessage();
            FileStream FS = new FileStream(xlPath, FileMode.Open, FileAccess.Read);
            ContentType CT = new ContentType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            Message.Attachments.Add(new Attachment(FS, Path.GetFileName(xlPath), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            Client.Send(Message);
        }
        private SmtpClient ConfigureSMTP()
        {
            SmtpClient smtp = new SmtpClient("smtp.office365.com");
            smtp.TargetName = "STARTTLS/smtp.office365.com";
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential(Configurator.MailAccount, Configurator.MailSecret);
            return smtp;
        }
        private MailMessage ConfigureMessage()
        {
            MailAddress from = new MailAddress(Configurator.MailAccount);
            MailAddress to = new MailAddress(Configurator.ReportEmails[0]);
            MailMessage message = new MailMessage(from, to);
            for (int i = 1; i < Configurator.ReportEmails.Length; i++) message.To.Add(Configurator.ReportEmails[i]);
            message.Subject = String.Format("Penguin Publishing production details for {0}.", DateTime.Now.ToString("F"));
            message.IsBodyHtml = true;
            return message;
        }
        private string BuildMessage()
        {
            string message = "Please find attached production report.";
            return message;
        }
    }
}
