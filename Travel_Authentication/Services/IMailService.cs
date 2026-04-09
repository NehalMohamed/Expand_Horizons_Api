

using Mails_App;
using Travel_Authentication.Controllers;

namespace Travel_Authentication.Services
{
    public interface IMailService
    {
        bool SendMail(MailData Mail_Data, ILogger _logger);
        //bool SendOTPMail(MailData Mail_Data);
        
    }

}
