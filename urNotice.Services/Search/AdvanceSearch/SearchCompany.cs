using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolrNet;
using urNotice.Common.Infrastructure.Model.Search.AdvanceSearch;
using urNotice.Common.Infrastructure.Model.urNoticeModel.Solr;
using urNotice.Services.Solr.SolrCompany;
using urNotice.Common.Infrastructure.Common.Constants;

namespace urNotice.Services.Search.AdvanceSearch
{
    public class SearchCompany : IAdvanceSearch
    {
        public AdvanceSearchResponse AdvanceSearch(AdvanceSearchRequest advanceSearchRequest)
        {
            ISolrCompany solrCompany = new SolrCompany();
            
            var fields = new[] { "description","displayname","logourl","minnoticeperiod","maxnoticeperiod","averagerating",
                        "employees","turnover","website","companyid" };

            advanceSearchRequest.page = string.IsNullOrEmpty(advanceSearchRequest.page) ? "0" : (Convert.ToInt32(advanceSearchRequest.page) - 1).ToString(CultureInfo.InvariantCulture);
            if (string.IsNullOrEmpty(advanceSearchRequest.perpage))
                advanceSearchRequest.perpage = "10";

            var query = BuildAdvanceSearchQuery(advanceSearchRequest);

            if (string.IsNullOrEmpty(advanceSearchRequest.totalMatch))
                advanceSearchRequest.totalMatch = solrCompany.ExecuteFromRestApi(query).ToString();

            var response = solrCompany.Execute(query, Convert.ToInt32(advanceSearchRequest.perpage), Convert.ToInt32(advanceSearchRequest.page) * Convert.ToInt32(advanceSearchRequest.perpage), fields);

            return AdvanceSearchModelAdapter(response, advanceSearchRequest.totalMatch);
        }

        private AdvanceSearchResponse AdvanceSearchModelAdapter(SolrQueryResults<UnCompanySolr> response,string resultCount)
        {
            var advanceSearchResponse = new AdvanceSearchResponse();
            advanceSearchResponse.searchResult = new List<AdvanceSearchObject>();
            foreach (var searchItem in response)
            {
                var advanceSearchObject = new AdvanceSearchObject()
                {
                    description = searchItem.description,
                    heading = searchItem.displayname,
                    logoUrl = searchItem.logourl,
                    minNoticeInfo = searchItem.minnoticeperiod.ToString(),
                    maxNoticeInfo = searchItem.maxnoticeperiod.ToString(),
                    ratingInfo = searchItem.averagerating.ToString(),
                    searchType = CommonConstants.Company,
                    sizeInfo = searchItem.employees,
                    turnOverInfo = searchItem.turnover,
                    url = searchItem.website,
                    vertexId = searchItem.companyid
                };
                advanceSearchResponse.searchResult.Add(advanceSearchObject);
            }
            advanceSearchResponse.searchCount = resultCount;
            return advanceSearchResponse;
        }

        private string BuildAdvanceSearchQuery(AdvanceSearchRequest advanceSearchRequest)
        {
            var solrQuery = "";
            switch (advanceSearchRequest.searchCriteria)
            {
                case SearchConstants.CONTAINS:
                    advanceSearchRequest.q = string.IsNullOrEmpty(advanceSearchRequest.q) ? "*" : advanceSearchRequest.q.Replace(" ", "*");
                    solrQuery = "(companyname:*" + advanceSearchRequest.q + "*)";
                    break;
                case SearchConstants.STARTSWITH:
                    advanceSearchRequest.q = string.IsNullOrEmpty(advanceSearchRequest.q) ? "*" : advanceSearchRequest.q.Replace(" ", "*");
                    solrQuery = "(companyname:" + advanceSearchRequest.q + "*)";
                    break;
                case SearchConstants.EXACTMATCH:
                    advanceSearchRequest.q = string.IsNullOrEmpty(advanceSearchRequest.q) ? "*" : advanceSearchRequest.q.Replace(" ", "?");
                    solrQuery = "(companyname:" + advanceSearchRequest.q + "*)";
                    break;
                default:
                    advanceSearchRequest.q = string.IsNullOrEmpty(advanceSearchRequest.q) ? "*" : advanceSearchRequest.q.Replace(" ", "*");
                    solrQuery = "(companyname:*" + advanceSearchRequest.q + "*)";
                    break;
            }
            
            return solrQuery;
        }
    }
}
