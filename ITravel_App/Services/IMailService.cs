

using Mails_App;

namespace ITravel_App.Services
{
    public interface IMailService
    {
        bool SendMail(MailData Mail_Data, ILogger _logger);
        bool ContactMail(MailData Mail_Data,string sender);
        
    }

}
