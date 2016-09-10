using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolrNet;
using urNotice.Common.Infrastructure.Model.SourceDB;
using urNotice.Common.Infrastructure.Model.urNoticeModel.DynamoDb;
using urNotice.Common.Infrastructure.Model.urNoticeModel.ResponseWrapper;
using urNotice.Common.Infrastructure.Model.urNoticeModel.Solr;

namespace urNotice.Services.Solr.SolrCompany
{
    public interface ISolrCompany : ISourceCompanyDb
    {
        SolrQueryResults<UnCompanySolr> GetCompanyDetailsAutocomplete(string queryText);
        SolrQueryResults<UnCompanySolr> GetAbsoluteCompanyDetailsAutocomplete(string queryText);
        SolrQueryResults<UnCompanySolr> CompanyDetailsById(string cid);
        SolrQueryResults<UnCompanySolr> Search(string q, string page, string perpage, ref string totalMatch);
        SolrQueryResults<UnCompanySolr> GetCompanyCompetitorsDetail(string size, string rating, string speciality);
        List<SearchAllResponseModel> SearchAllAutocomplete(string queryText,int from,int to);
        SolrQueryResults<UnCompanySolr> Execute(string query, int rows, int start, string[] fields);
        long ExecuteFromRestApi(string query);
    }
}
