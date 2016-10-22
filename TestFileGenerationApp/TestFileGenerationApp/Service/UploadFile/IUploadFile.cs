using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestFileGenerationApp.Service.UploadFile
{
    public interface IUploadFile
    {
        string execute(HttpRequestBase Request, HttpServerUtilityBase Server);
    }
}