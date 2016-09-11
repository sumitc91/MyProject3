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
                advanceSearchRequest.totalMatch = solrCompany.ExecuteFromRestApi(query.Replace("+","%2B")).ToString();

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
            solrQuery = ApplySearchCriteriaOnQuery(advanceSearchRequest);
            solrQuery = ApplyRatingCriteriaOnQuery(advanceSearchRequest, solrQuery);
            solrQuery = ApplyCompanySizeCriteriaOnQuery(advanceSearchRequest, solrQuery);
            solrQuery = ApplyCompanyTurnoverCriteriaOnQuery(advanceSearchRequest, solrQuery);
            return solrQuery;
        }

        private static string ApplyCompanyTurnoverCriteriaOnQuery(AdvanceSearchRequest advanceSearchRequest, string solrQuery)
        {
            string[] companyTurnoverArray;
            bool isLastIteration = false;
            if (!string.IsNullOrEmpty(advanceSearchRequest.companyTurnOver))
            {
                companyTurnoverArray = advanceSearchRequest.companyTurnOver.Split(',');
                solrQuery += " AND ( ";

                for (int i = 0; i < companyTurnoverArray.Length; i++)
                {
                    isLastIteration = (i == companyTurnoverArray.Length - 1);
                    switch (companyTurnoverArray[i])
                    {
                        case SearchConstants.RANGE10000PLUS:
                            solrQuery += " (turnover:10000+)";
                            solrQuery += !isLastIteration ? " OR" : "";
                            break;
                        case SearchConstants.RANGE5000TO10000:
                            solrQuery += " (turnover:5000?-?10000)";
                            solrQuery += !isLastIteration ? " OR" : "";
                            break;
                        case SearchConstants.RANGE2500TO5000:
                            solrQuery += " (turnover:2500?-?5000)";
                            solrQuery += !isLastIteration ? " OR" : "";
                            break;
                        case SearchConstants.RANGE1000TO2500:
                            solrQuery += " (turnover:1000?-?2500)";
                            solrQuery += !isLastIteration ? " OR" : "";
                            break;
                        case SearchConstants.RANGE500TO1000:
                            solrQuery += " (turnover:500?-?1000)";
                            solrQuery += !isLastIteration ? " OR" : "";
                            break;
                        case SearchConstants.RANGE100TO500:
                            solrQuery += " (turnover:100?-?500)";
                            solrQuery += !isLastIteration ? " OR" : "";
                            break;
                        case SearchConstants.RANGE1TO100:
                            solrQuery += " (turnover:1?-?100)";
                            solrQuery += !isLastIteration ? " OR" : "";
                            break;
                    }
                }
                solrQuery += " )";
            }

            return solrQuery;
        }
        private static string ApplyCompanySizeCriteriaOnQuery(AdvanceSearchRequest advanceSearchRequest, string solrQuery)
        {
            string[] companySizeArray;
            bool isLastIteration = false;
            if (!string.IsNullOrEmpty(advanceSearchRequest.companySize))
            {
                companySizeArray = advanceSearchRequest.companySize.Split(',');
                solrQuery += " AND ( ";

                for (int i = 0; i < companySizeArray.Length; i++)
                {
                    isLastIteration = (i == companySizeArray.Length - 1);
                    switch (companySizeArray[i])
                    {
                        case SearchConstants.RANGE10001PLUS:
                            solrQuery += " (employees:10001+)";
                            solrQuery += !isLastIteration ? " OR" : "";
                            break;
                        case SearchConstants.RANGE5001TO10000:
                            solrQuery += " (employees:5001?-?10000)";
                            solrQuery += !isLastIteration ? " OR" : "";
                            break;
                        case SearchConstants.RANGE1001TO5000:
                            solrQuery += " (employees:1001?-?5000)";
                            solrQuery += !isLastIteration ? " OR" : "";
                            break;
                        case SearchConstants.RANGE501TO1000:
                            solrQuery += " (employees:501?-?1000)";
                            solrQuery += !isLastIteration ? " OR" : "";
                            break;
                        case SearchConstants.RANGE201TO500:
                            solrQuery += " (employees:201?-?500)";
                            solrQuery += !isLastIteration ? " OR" : "";
                            break;
                        case SearchConstants.RANGE51TO200:
                            solrQuery += " (employees:51?-?200)";
                            solrQuery += !isLastIteration ? " OR" : "";
                            break;
                        case SearchConstants.RANGE11TO50:
                            solrQuery += " (employees:11?-?50)";
                            solrQuery += !isLastIteration ? " OR" : "";
                            break;
                        case SearchConstants.RANGE1TO10:
                            solrQuery += " (employees:1?-?10)";
                            solrQuery += !isLastIteration ? " OR" : "";
                            break;
                    }
                }
                solrQuery += " )";
            }

            return solrQuery;
        }
        private static string ApplyRatingCriteriaOnQuery(AdvanceSearchRequest advanceSearchRequest, string solrQuery)
        {
            string[] ratingArray;
            bool isLastIteration = false;
            if (!string.IsNullOrEmpty(advanceSearchRequest.rating))
            {
                ratingArray = advanceSearchRequest.rating.Split(',');
                solrQuery += " AND ( ";

                for (int i = 0; i < ratingArray.Length; i++)
                {
                    isLastIteration = (i == ratingArray.Length - 1);
                    switch (ratingArray[i])
                    {
                        case SearchConstants.RANGE4PLUS:
                            solrQuery += " (averagerating:[4.0 TO *])";
                            solrQuery += !isLastIteration ? " OR" : "";
                            break;
                        case SearchConstants.RANGE3TO4:
                            solrQuery += " (averagerating:[3.0 TO 4.0])";
                            solrQuery += !isLastIteration ? " OR" : "";
                            break;
                        case SearchConstants.RANGE2TO3:
                            solrQuery += " (averagerating:[2.0 TO 3.0])";
                            solrQuery += !isLastIteration ? " OR" : "";
                            break;
                        case SearchConstants.RANGE1T02:
                            solrQuery += " (averagerating:[1.0 TO 2.0])";
                            solrQuery += !isLastIteration ? " OR" : "";
                            break;
                    }
                }
                solrQuery += " )";
            }

            return solrQuery;
        }
        private static string ApplySearchCriteriaOnQuery(AdvanceSearchRequest advanceSearchRequest)
        {
            string solrQuery;
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
