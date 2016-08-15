using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using urNotice.Common.Infrastructure.Common.Constants;
using urNotice.Services.Management.NotificationManagement;

namespace urNotice.Services.Factory.NotificationManagement
{
    public class NotificationManagementFactory
    {
        public static INotificationManagement GetNotificationManagement(string version)
        {
            switch (version)
            {
                case OrbitPageVersionConstants.v1:
                    return new NotificationManagementV1();

                default:
                    return new NotificationManagementV1();
            }
        }
    }
}
