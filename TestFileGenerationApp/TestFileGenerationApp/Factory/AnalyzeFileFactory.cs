using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestFileGenerationApp.Models.Enum;
using TestFileGenerationApp.Service.AnalyzeFile;
using TestFileGenerationApp.Service.ProcessFile;
using TestFileGenerationApp.Service.UploadFile;

namespace TestFileGenerationApp.Factory
{
    public class AnalyzeFileFactory
    {
        public static IAnalyzeFile CreateInstance(FileTypeEnum fileType)
        {
            switch (fileType)
            {
                case FileTypeEnum.CSharp:
                    return new AnalyzeCsFile();

                case FileTypeEnum.Javascript:
                    return new AnalyzeJavascriptFile();

                default:
                    return new AnalyzeCsFile();
            }
        }
    }
}