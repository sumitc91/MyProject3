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
    public class ProcessCSharpFile : IProcessFile
    {
        IUploadFile uploadFileService;
        IAnalyzeFile analyzeFileService;
        ICreateFile createFileService;
        public string uploadFile(IUploadFile uploadFileService, HttpRequestBase Request, HttpServerUtilityBase Server)
        {
            this.uploadFileService = uploadFileService;
            return this.uploadFileService.execute(Request, Server);
        }

        public string createFile(ClassAnalyzedModel classAnalyzedData, HttpServerUtilityBase Server, ICreateFile createFileService)
        {
            this.createFileService = createFileService;
            return createFileService.execute(classAnalyzedData, Server);
        }

        public ClassAnalyzedModel analyzeFile(string path, IAnalyzeFile analyzeFileService)
        {
            this.analyzeFileService = new AnalyzeCsFile();
            return this.analyzeFileService.execute(path);
        }
    }
}