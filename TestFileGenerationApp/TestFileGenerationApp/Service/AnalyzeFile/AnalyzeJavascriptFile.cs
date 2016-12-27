using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using TestFileGenerationApp.Models;

namespace TestFileGenerationApp.Service.AnalyzeFile
{
    public class AnalyzeJavascriptFile : IAnalyzeFile
    {
        public ClassAnalyzedModel execute(string strFilePath)
        {
            var classAnalyzedModel = new ClassAnalyzedModel();

            List<string> methodNames = new List<string>();
            classAnalyzedModel.usedNamespacesStr = new List<string>();
            classAnalyzedModel.usedNamespacesStr = File.ReadAllLines(strFilePath)
                                        .Where(a => (a.Contains(".controller("))).Distinct().ToList();

            //classAnalyzedModel.classNamespaceStr = File.ReadAllLines(strFilePath)
            //                            .Where(a => (a.Contains("namespace "))).FirstOrDefault().Replace("namespace ", "");
            //classAnalyzedModel.classNameStr = File.ReadAllLines(strFilePath)
            //                            .Where(a => (a.Contains("class "))).FirstOrDefault().Split(':')[0].TrimEnd().Split(' ').Last();

            var fileStr = File.ReadAllText(strFilePath);

            string pattern = @"(?<=.controller\()(.*)(?=\))";
            Regex rx = new Regex(pattern, RegexOptions.None);
            MatchCollection mc = rx.Matches(fileStr);

            classAnalyzedModel.classNameStr = mc[0].Value.Split(',')[0].Replace("\"","") ;


            string pattern2 = @"(?<=, ?function ?\()(.*)(?=\))";
            Regex rx2 = new Regex(pattern2, RegexOptions.None);
            MatchCollection mc2 = rx2.Matches(fileStr);

            classAnalyzedModel.instancesStr = new List<string>();
            foreach (Match m in mc2)
            {
                classAnalyzedModel.instancesStr.AddRange(m.Value.Split(','));
            }

            //$scope.showPageResetConfirmationPopup = function ()
            string pattern3 = @"(?<=\$scope.)(.*)(?= = ?function ?\(\))";
            //string pattern3 = @"\$scope.(.*)= ?function\(\)";
            Regex rx3 = new Regex(pattern3, RegexOptions.None);
            MatchCollection mc3 = rx3.Matches(fileStr);
            classAnalyzedModel.methodStr = new List<string>();
            foreach (Match m in mc3)
            {
                classAnalyzedModel.methodStr.Add(m.Value);
            }

            

            return classAnalyzedModel;
        }
    }
}