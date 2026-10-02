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
        public void SendMail(string livePath, string shippedPath)
        {
            Message.Body = BuildMessage();
            using FileStream liveStream = new FileStream(livePath, FileMode.Open, FileAccess.Read);
            using FileStream shippedStream = new FileStream(shippedPath, FileMode.Open, FileAccess.Read);
            //ContentType CT = new ContentType("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            Message.Attachments.Add(new Attachment(liveStream, Path.GetFileName(livePath), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
            Message.Attachments.Add(new Attachment(shippedStream, Path.GetFileName(shippedPath), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
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
            string message = "Please find attached production reports for live and shipped POs.";
            return message;
        }
    }
}
