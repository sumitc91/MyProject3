using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestFileGenerationApp.Models.Enum;
using TestFileGenerationApp.Service.ProcessFile;

namespace TestFileGenerationApp.Factory
{
    public class ProcessFileFactory
    {
        public static IProcessFile CreateInstance(FileTypeEnum fileType)
        {
            switch (fileType)
            {
                case FileTypeEnum.CSharp:
                    return new ProcessCSharpFile();
                    
                default:
                    return new ProcessCSharpFile();
            }
        }
    }
}