using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using urNotice.Common.Infrastructure.Common.Constants;
using urNotice.Services.Management.AccountManagement;

namespace urNotice.Services.Factory.AccountManagement
{
    public class AccountManagementFactory
    {
        public static IAccountManagement GetAccountManagementInstance(string version)
        {
            switch (version)
            {
                case OrbitPageVersionConstants.v1:
                    return new AccountManagementV1();
                    
                default:
                    return new AccountManagementV1();
            }
        }
    }
}
