using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TestFileGenerationApp.Service.UploadFile
{
    public class UploadJavascriptFile : IUploadFile
    {
        public string execute(HttpRequestBase Request, HttpServerUtilityBase Server)
        {
            // Checking no of files injected in Request object  
            string fname="";
            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                        file.SaveAs(fname);
                    }
                    // Returns message that successfully uploaded  
                    return fname;
                }
                catch (Exception ex)
                {
                    return "Error occurred. Error details: " + ex.Message;
                }
            }
            else
            {
                return "No files selected.";
            }
        }
    }
}