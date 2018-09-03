using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace yavc.DiagnosticTool
{
    public class SimpleMailer
    {
        private const string FROM = @"Temp@MasterDevs.com";

        public static bool SendMail(string to, string subject, string body) {
            return SendMail(to, subject, body, string.Empty);
        }

        public static bool SendMail(string to, string subject, string body, string fileName)
        {
            try
            {
                using (var message = new MailMessage(FROM, to, subject, body))
                {
                    message.Subject = "Diagnostics";

                    TryAddAttachment(message, fileName);
                    
                    var client = new SmtpClient(@"smtp.gmail.com", 587);
                    client.Credentials = new NetworkCredential(FROM, "temp1234!@#$");
                    client.EnableSsl = true;
                    client.Send(message);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        private static void TryAddAttachment(MailMessage m, string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) return; //-- Can't send nothing.

            var attachement = new Attachment(fileName, MediaTypeNames.Application.Octet);

            var contentDisposition = attachement.ContentDisposition;
            contentDisposition.CreationDate = File.GetCreationTime(fileName);
            contentDisposition.ModificationDate = File.GetLastWriteTime(fileName);
            contentDisposition.ReadDate = File.GetLastAccessTime(fileName);
            
            m.Attachments.Add(attachement);
        }
    }
}
