using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using urNotice.Common.Infrastructure.Common.Config;
using urNotice.Common.Infrastructure.Common.Constants;
using urNotice.Common.Infrastructure.Common.Constants.EmailConstants;
using urNotice.Common.Infrastructure.Common.Enum;
using urNotice.Common.Infrastructure.commonMethods;
using urNotice.Common.Infrastructure.Model.urNoticeModel.RequestWrapper;
using urNotice.Services.ErrorLogger;
using urNotice.Services.Factory.Email;

namespace urNotice.Services.Email.EmailTemplate
{
    public class SendAccountCreationValidationEmail
    {
        private ILogger _logger = new Logger(Convert.ToString(MethodBase.GetCurrentMethod().DeclaringType));
        
        public SendAccountCreationValidationEmail(ILogger logger)
        {
            _logger = logger;
        }

        public static void SendAccountCreationValidationEmailMessage(RegisterationRequest req, String guid, HttpRequestBase request)
        {
            
            if (request.Url != null)
            {                
                IEmail emailModel = EmailFactory.GetEmailInstance(SmtpConfig.ActiveEmailForAccountCreationValidation);
                emailModel.SendEmail(req.EmailId,
                    SmptCreateAccountConstants.SenderName,
                    SmptCreateAccountConstants.EmailTitle,
                    CreateAccountEmailBodyContent(request.Url.Authority, req, guid),
                    null,
                    null,
                    SmptCreateAccountConstants.SenderName,
                    null
                    );
            }
        }

        public static void SendAccountValidationEmailMessage(String toMail, String guid, HttpRequestBase request)
        {
            if (request != null && request.Url != null)
            {
                IEmail emailModel = EmailFactory.GetEmailInstance(SmtpConfig.ActiveEmailForAccountCreationValidation);
                emailModel.SendEmail(toMail,
                    SmptCreateAccountConstants.SenderName,
                    SmptCreateAccountConstants.EmailTitle,
                    CreateAccountEmailBodyContent(request.Url.Authority,toMail, guid),
                    null,
                    null,
                    SmptCreateAccountConstants.SenderName,
                    null
                    );
            }
        }
        
        private static string CreateAccountEmailBodyContent(String requestUrlAuthority, RegisterationRequest req, String guid)
        {
            var template = File.ReadAllText(HttpContext.Current.Server.MapPath("~/EmailTemplate/CreateAccountEmail.html"));

            var encryptedUserInfo = new Dictionary<String, String>();
            encryptedUserInfo["EMAIL"] = req.EmailId;
            encryptedUserInfo["KEY"] = guid;

            string messageBody = template.Replace("{1}", "http://" + SmptCreateAccountConstants.AccountDomain + "/#/" + "validate/" + encryptedUserInfo["EMAIL"] + "/" + encryptedUserInfo["KEY"]);
            return messageBody;   
        }

        private static string CreateAccountEmailBodyContent(String requestUrlAuthority, String email, String guid)
        {
            var template = File.ReadAllText(HttpContext.Current.Server.MapPath("~/EmailTemplate/CreateAccountEmail.html"));

            var encryptedUserInfo = new Dictionary<String, String>();
            encryptedUserInfo["EMAIL"] = email;
            encryptedUserInfo["KEY"] = guid;

            string messageBody = template.Replace("{1}", "http://" + SmptCreateAccountConstants.AccountDomain + "/#/" + "validate/" + encryptedUserInfo["EMAIL"] + "/" + encryptedUserInfo["KEY"]);
            return messageBody;
        }
        
    }
}