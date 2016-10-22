using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestFileGenerationApp.Models;

namespace TestFileGenerationApp.Service.CreateFile
{
    public interface ICreateFile
    {
        string execute(ClassAnalyzedModel classAnalyzedModel, HttpServerUtilityBase Server);
    }
}