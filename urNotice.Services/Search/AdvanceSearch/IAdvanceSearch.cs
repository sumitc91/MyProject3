using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using urNotice.Common.Infrastructure.Model.Search.AdvanceSearch;

namespace urNotice.Services.Search.AdvanceSearch
{
    public interface IAdvanceSearch
    {
        AdvanceSearchResponse AdvanceSearch(AdvanceSearchRequest advanceSearchRequest);
    }
}
