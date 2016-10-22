using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestFileGenerationApp.Models
{
    public class CreateFileResponseModel
    {
        public string uploadedFile { get; set; }
        public string createdFile { get; set; }

        public string fileName { get; set; }
    }
}