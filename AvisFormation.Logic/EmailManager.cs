using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace AvisFormation.Logic
{
    public class EmailManager
    {
        public void SendEmail(string titre, string message, string email)
        {
            MailMessage msg = GetMailMessage(
                new MailAddress(email),
                new MailAddress("administrateur@avisformation.fr"),
                "Nouveau message AvisFormations",
                message
            );

            SendMail(msg);
        }

        public MailMessage GetMailMessage(MailAddress from, MailAddress to, string subject, string body)
        {
            MailMessage msg = new MailMessage();
            msg.From = from;
            msg.To.Add(to);
            msg.Subject = subject;
            msg.Body = body;

            return msg;
        }


        // Ajouter les valeurs "fake" au fichier de config
        public SmtpClient GetSmtpClient()
        {
            return new SmtpClient
            {
                Host = "ns0.ovh.net",
                Port = 587,
                EnableSsl = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Timeout = 30 * 1000,
                Credentials = new NetworkCredential("", "")
            };
        }

        public void SendMail(MailMessage message)
        {
            GetSmtpClient().Send(message);
        }
    }
}
