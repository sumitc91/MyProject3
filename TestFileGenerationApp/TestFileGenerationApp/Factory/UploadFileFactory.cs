using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestFileGenerationApp.Models.Enum;
using TestFileGenerationApp.Service.ProcessFile;
using TestFileGenerationApp.Service.UploadFile;

namespace TestFileGenerationApp.Factory
{
    public class UploadFileFactory
    {
        public static IUploadFile CreateInstance(FileTypeEnum fileType)
        {
            switch (fileType)
            {
                case FileTypeEnum.CSharp:
                    return new UploadCsharpFile();
                    
                default:
                    return new UploadCsharpFile();
            }
        }
    }
}