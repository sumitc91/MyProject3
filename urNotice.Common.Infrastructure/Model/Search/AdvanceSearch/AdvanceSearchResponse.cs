using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace urNotice.Common.Infrastructure.Model.Search.AdvanceSearch
{
    public class AdvanceSearchResponse
    {
        public List<AdvanceSearchObject> searchResult { get; set; }
        public string searchCount { get; set; }
        
    }
}
