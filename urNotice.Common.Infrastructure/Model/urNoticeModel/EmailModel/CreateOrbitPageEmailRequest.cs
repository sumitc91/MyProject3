using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urNotice.Common.Infrastructure.Model.urNoticeModel.EmailModel
{
    public class CreateOrbitPageEmailRequest
    {
        public string fromEmail { get; set; }
        public string fromName { get; set; }
        public string sendToEmail { get; set; }
        public string password { get; set; }
        public string emailHeading { get; set; }
        public string emailBody { get; set; }
    }
}
