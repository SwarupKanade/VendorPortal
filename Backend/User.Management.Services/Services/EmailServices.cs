using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Management.Services.Models;

namespace User.Management.Services.Services
{
    public class EmailServices : IEmailServices
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailServices(EmailConfiguration emailConfig) 
        {
            _emailConfig = emailConfig;
        }
        public void SendMail(Message message)
        {
            var msg=CreateEmailMessage(message);
            Send(msg);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var msg=new MimeMessage();
            msg.From.Add(new MailboxAddress("email",_emailConfig.From));
            msg.To.AddRange(message.To);
            msg.Subject=message.Subject;
            msg.Body=new TextPart(MimeKit.Text.TextFormat.Text) { Text=message.Content };
            return msg;
        }

        private void Send(MimeMessage msg)
        {
            using var client = new SmtpClient();

            try
            {
                client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfig.Username, _emailConfig.Password);
                client.Send(msg);
            }
            catch
            {
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }
    }
}
