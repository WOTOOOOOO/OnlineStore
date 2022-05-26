using someOnlineStore.Data.EmailData;

namespace someOnlineStore.Data.Services.ServiceInterfaces
{
    public interface IMailService
    {
        public Task sendEmail(Message message);
    }
}
