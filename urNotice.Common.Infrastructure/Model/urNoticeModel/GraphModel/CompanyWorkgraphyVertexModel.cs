using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using urNotice.Common.Infrastructure.Model.urNoticeModel.GraphModel.V1;

namespace urNotice.Common.Infrastructure.Model.urNoticeModel.GraphModel
{
    public class CompanyWorkgraphyVertexModel
    {
        public List<CompanyWorkgraphyInfoVertexModel> workgraphyInfo { get; set; }
        public string count { get; set; }
        public string userCount { get; set; }
    }

    public class CompanyWorkgraphyVertexModelV1
    {
        public VertexModelV1 workgraphyinfo { get; set; }
        public VertexModelV1 workgraphystoryinfo { get; set; }
        public long count { get; set; }
        public long userCount { get; set; }
    }
}
