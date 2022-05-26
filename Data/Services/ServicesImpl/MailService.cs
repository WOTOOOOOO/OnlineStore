using someOnlineStore.Data.EmailData;
using someOnlineStore.Data.Services.ServiceInterfaces;
using System.Net;
using System.Net.Mail;


namespace someOnlineStore.Data.Services.ServicesImpl
{
    public class MailService : IMailService
    {
        private readonly EmailConfiguration _emailConfig;

        public MailService(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);

            Send(emailMessage);
        }

        public async Task sendEmail(Message message)
        {
            var mailMessage = CreateEmailMessage(message);

            await SendAsync(mailMessage);
        }

        private MailMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MailMessage();
            emailMessage.From = new MailAddress(_emailConfig.From);
            message.To.ForEach( to => { emailMessage.To.Add(to); });
            emailMessage.Subject = message.Subject;
            emailMessage.IsBodyHtml = true;
            emailMessage.Body = message.Content;
            if (message.Attachments != null && message.Attachments.Any())
            {
                byte[] fileBytes;
                foreach (var attachment in message.Attachments)
                {
                    var ms = new MemoryStream();
                        attachment.CopyTo(ms);
                    emailMessage.Attachments.Add(new Attachment(ms, attachment.FileName, attachment.ContentType));
                }
            }

            
            return emailMessage;
        }

        private void Send(MailMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Port = 587;
                    client.Host = "smtp.gmail.com"; //for gmail host  
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_emailConfig.From, _emailConfig.Password);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    client.Dispose();
                }
            }
        }

        private async Task SendAsync(MailMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Port = 587;
                    client.Host = "smtp.gmail.com"; //for gmail host  
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_emailConfig.From, _emailConfig.Password);
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    await client.SendMailAsync(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    
                    client.Dispose();
                }
            }
        }
    }
}
