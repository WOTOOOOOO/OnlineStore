using System.Net.Mail;

namespace someOnlineStore.Data.EmailData
{
    public class Message
    {
        public List<MailAddress> To { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        public IFormFileCollection? Attachments { get; set; }

        public Message(IEnumerable<string> to, string subject, string content, IFormFileCollection? attachments)
        {
            To = new List<MailAddress>();

            To.AddRange(to.Select(x => new MailAddress(x)));
            Subject = subject;
            Content = content;
            Attachments = attachments;
        }
    }
}
