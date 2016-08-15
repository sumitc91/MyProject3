using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using urNotice.Common.Infrastructure.Common.Constants;
using urNotice.Services.Management.PostManagement;

namespace urNotice.Services.Factory.PostManagement
{
    public class PostManagementFactory
    {
        public static IPostManagement GetPostManagementInstance(string version)
        {
            switch (version)
            {
                case OrbitPageVersionConstants.v1:
                    return new PostManagementV1();

                default:
                    return new PostManagementV1();
            }
        }
    }
}
