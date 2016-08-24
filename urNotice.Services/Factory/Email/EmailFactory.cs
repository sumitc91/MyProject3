using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using urNotice.Common.Infrastructure.Common.Constants;
using urNotice.Common.Infrastructure.Common.Enum;
using urNotice.Services.Email;
using urNotice.Services.Email.EmailFromGmail;
using urNotice.Services.Email.EmailFromMandrill;

namespace urNotice.Services.Factory.Email
{
    public class EmailFactory
    {
        public static IEmail GetEmailInstance(EmailSourceEnum type)
        {
            switch (type)
            {
                case EmailSourceEnum.GMAIL:
                    return new EmailFromGmail();
                case EmailSourceEnum.MANDRILL:
                    return new EmailFromMandrill();
                default:
                    return new EmailFromGmail();
            }
        }
        
    }
}
