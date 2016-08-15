using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using urNotice.Common.Infrastructure.Common.Constants;
using urNotice.Services.Management.CompanyManagement;

namespace urNotice.Services.Factory.CompanyManagement
{
    public class CompanyManagementFactory
    {
        public static ICompanyManagement GetCompanyManagementInstance(string version)
        {
            switch (version)
            {
                case OrbitPageVersionConstants.v1:
                    return new CompanyManagementV1();

                default:
                    return new CompanyManagementV1();
            }
        }
    }
}
