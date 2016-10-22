using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestFileGenerationApp.Models;

namespace TestFileGenerationApp.Service.AnalyzeFile
{
    public interface IAnalyzeFile
    {
        ClassAnalyzedModel execute(string strFilePath);
    }
}