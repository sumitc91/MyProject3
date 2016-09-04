using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace urNotice.Common.Infrastructure.Session
{
    public class HeaderManager
    {
        public HeaderManager(HttpRequestBase requestHeader)
        {
            IEnumerable<string> headerValues = requestHeader.Headers.GetValues("UTMZT");
            if(headerValues != null)
                this.AuthToken = headerValues.FirstOrDefault();            
            
            headerValues = requestHeader.Headers.GetValues("UTMZK");            
            if (headerValues != null)
                this.AuthKey = headerValues.FirstOrDefault();

            headerValues = requestHeader.Headers.GetValues("UTMZV");
            if (headerValues != null)
                this.AuthValue = headerValues.FirstOrDefault();
            
            headerValues = requestHeader.Headers.GetValues("_ga");
            if(headerValues != null)
                this.Ga = headerValues.FirstOrDefault();

            if(this.AuthToken == null || this.AuthKey == null)
            {
                CheckAndAssignFromCookieIfHeaderIsEmpty(requestHeader);
            }
        }

        private void CheckAndAssignFromCookieIfHeaderIsEmpty(HttpRequestBase requestHeader)
        {
            if (requestHeader.Cookies["UTMZT"] != null)
            {
                this.AuthToken = DecodeHtmlCookie(requestHeader.Cookies["UTMZT"].Value);
            }

            if (requestHeader.Cookies["UTMZK"] != null)
            {
                this.AuthKey = DecodeHtmlCookie(requestHeader.Cookies["UTMZK"].Value);
            }

            if (requestHeader.Cookies["UTMZV"] != null)
            {
                this.AuthValue = DecodeHtmlCookie(requestHeader.Cookies["UTMZV"].Value);
            }

            if (requestHeader.Cookies["_ga"] != null)
            {
                this.Ga = DecodeHtmlCookie(requestHeader.Cookies["_ga"].Value);
            }
        }

        private string DecodeHtmlCookie(string str)
        {
            return str
                .Replace("%2F", "/")
                .Replace("%2B", "+")
                .Replace("%3D", "=");
        }
        public string AuthToken { get; set; }
        public string AuthKey { get; set; }
        public string AuthValue { get; set; }
        public string Ga { get; set; }
    }
}