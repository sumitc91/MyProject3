using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urNotice.Common.Infrastructure.Model.Search.AdvanceSearch
{
    public class AdvanceSearchRequest
    {
        public string q { get; set; }
        public string page { get; set; }
        public string perpage { get; set; }
        public string searchType { get; set; }
        public string searchCriteria { get; set; }
        public string rating { get; set; }
        public string companySize { get; set; }
        public string companyTurnOver { get; set; }
        public string userMutualFriendsType { get; set; }
        public string datePosted { get; set; }
        public string totalMatch { get; set; }
    }
}
