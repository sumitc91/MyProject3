using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestFileGenerationApp.Factory;
using TestFileGenerationApp.Models;
using TestFileGenerationApp.Models.Enum;
using TestFileGenerationApp.Service.AnalyzeFile;
using TestFileGenerationApp.Service.CreateFile;
using TestFileGenerationApp.Service.ProcessFile;
using TestFileGenerationApp.Service.UploadFile;

namespace TestFileGenerationApp.Controllers
{
    public class HomeController : Controller
    {
        public object IUploadfile { get; private set; }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateTestFile()
        {
            var respone = new CreateFileResponseModel();

            var fileType = Request.Files.Count>0 ? Request.Files[0].FileName : "File Not Available";

            FileTypeEnum fileTypeEnum = fileType.Contains(".js") ? FileTypeEnum.Javascript : FileTypeEnum.CSharp;

            respone.extension = fileType.Contains(".js") ? "js" : "cs";
            IProcessFile processFile = ProcessFileFactory.CreateInstance(fileTypeEnum);

            IUploadFile uploadFile = UploadFileFactory.CreateInstance(fileTypeEnum);
            var uploadedPath = processFile.uploadFile(uploadFile,Request,Server);

            IAnalyzeFile analyzeFile = AnalyzeFileFactory.CreateInstance(fileTypeEnum);
            var classAnalyzedData = processFile.analyzeFile(uploadedPath,analyzeFile);

            ICreateFile createFile = CreateFileFactory.CreateInstance(fileTypeEnum);
            var createdPath = processFile.createFile(classAnalyzedData, Server,createFile); 

            respone.createdFile = System.IO.File.ReadAllText(createdPath);
            respone.uploadedFile = System.IO.File.ReadAllText(uploadedPath);
            respone.fileName = classAnalyzedData.classNameStr;
            return Json(respone);
        }

        public FileResult Download()
        {
            String fileName = Request.QueryString["fileName"];
            string path = Path.Combine(Server.MapPath("~/TestUploads/"), fileName);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
    }
}