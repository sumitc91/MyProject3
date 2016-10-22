using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestFileGenerationApp.Models;
using TestFileGenerationApp.Service.AnalyzeFile;
using TestFileGenerationApp.Service.CreateFile;
using TestFileGenerationApp.Service.UploadFile;

namespace TestFileGenerationApp.Service.ProcessFile
{
    public interface IProcessFile
    {
        string uploadFile(IUploadFile uploadFileService, HttpRequestBase Request, HttpServerUtilityBase Server);
        ClassAnalyzedModel analyzeFile(string path, IAnalyzeFile analyzeFileService);
        string createFile(ClassAnalyzedModel classAnalyzedData, HttpServerUtilityBase Server, ICreateFile createFileService);
    }
}