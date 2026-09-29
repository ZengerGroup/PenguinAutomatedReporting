using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace PenguinAutomatedReporting
{
    internal static class Configurator
    {
        public static string LogPath = ConfigurationManager.AppSettings["LogPath"];
        public static string IssuePath = ConfigurationManager.AppSettings["IssuePath"];
        public static string CredentialsPath = ConfigurationManager.AppSettings["Credentials"];
        public static string SheetId = ConfigurationManager.AppSettings["SheetId"];
        public static string MailAccount = ConfigurationManager.AppSettings["MailAccount"];
        public static string MailSecret = ConfigurationManager.AppSettings["MailSecret"];
        public static string[] ReportEmails = ConfigurationManager.AppSettings["ReportEmails"].Split("|");
    }
}
