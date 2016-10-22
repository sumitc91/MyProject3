using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestFileGenerationApp.Models.Enum;
using TestFileGenerationApp.Service.CreateFile;
using TestFileGenerationApp.Service.ProcessFile;
using TestFileGenerationApp.Service.UploadFile;

namespace TestFileGenerationApp.Factory
{
    public class CreateFileFactory
    {
        public static ICreateFile CreateInstance(FileTypeEnum fileType)
        {
            switch (fileType)
            {
                case FileTypeEnum.CSharp:
                    return new CreateCsharpFile();
                    
                default:
                    return new CreateCsharpFile();
            }
        }
    }
}