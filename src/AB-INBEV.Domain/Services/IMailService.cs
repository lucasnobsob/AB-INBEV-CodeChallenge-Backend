using AB_INBEV.Domain.Services.Mail;

namespace AB_INBEV.Domain.Services
{
    public interface IMailService
    {
        void SendMail(MailMessage mailMessage);
    }
}
