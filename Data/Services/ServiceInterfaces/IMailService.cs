using someOnlineStore.Data.EmailData;

namespace someOnlineStore.Data.Services.ServiceInterfaces
{
    public interface IMailService
    {

        public void SendEmail(Message message);
        public Task SendEmailAsync(Message message);
    }
}
