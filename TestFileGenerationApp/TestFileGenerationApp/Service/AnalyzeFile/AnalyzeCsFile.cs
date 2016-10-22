using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using TestFileGenerationApp.Models;

namespace TestFileGenerationApp.Service.AnalyzeFile
{
    public class AnalyzeCsFile : IAnalyzeFile
    {
        public ClassAnalyzedModel execute(string strFilePath)
        {
            var classAnalyzedModel = new ClassAnalyzedModel();

            List<string> methodNames = new List<string>();
            classAnalyzedModel.usedNamespacesStr = new List<string>();
            classAnalyzedModel.usedNamespacesStr = File.ReadAllLines(strFilePath)
                                        .Where(a => (a.Contains("using "))).Distinct().ToList();

            classAnalyzedModel.classNamespaceStr = File.ReadAllLines(strFilePath)
                                        .Where(a => (a.Contains("namespace "))).FirstOrDefault().Replace("namespace ", "");
            classAnalyzedModel.classNameStr = File.ReadAllLines(strFilePath)
                                        .Where(a => (a.Contains("class "))).FirstOrDefault().Split(':')[0].TrimEnd().Split(' ').Last();

            var fileStr = File.ReadAllText(strFilePath);

            string pattern = @"TryCreateOrGet<(.*?)>\(\)";
            Regex rx = new Regex(pattern, RegexOptions.None);
            MatchCollection mc = rx.Matches(fileStr);

            classAnalyzedModel.instancesStr = new List<string>();
            foreach (Match m in mc)
            {
                classAnalyzedModel.instancesStr.Add(m.Value.Replace("TryCreateOrGet<", "").Replace(">()", ""));
            }
            classAnalyzedModel.instancesStr = classAnalyzedModel.instancesStr.Distinct().ToList();

            classAnalyzedModel.methodStr = File.ReadAllLines(strFilePath)
                                        .Where(a => (a.Contains("protected") ||
                                                    a.Contains("private") ||
                                                    a.Contains("internal") ||
                                                    a.Contains("public")) &&
                                                    !a.Contains(" " + classAnalyzedModel.classNameStr + "Controller") &&
                                                    !a.Contains("set;") &&
                                                    !a.Contains("abstract ") &&
                                                    !a.Contains("_") && !a.Contains("class")).Distinct().ToList();

            classAnalyzedModel.methodListObj = new List<ClassAnalyzedModelMethodInfo>();
            for (int j = 0; j < classAnalyzedModel.methodStr.Count; j++)
            {
                var methodInfo = new ClassAnalyzedModelMethodInfo();
                classAnalyzedModel.methodStr[j] = classAnalyzedModel.methodStr[j].Replace("()","( )");
                foreach (Match match in Regex.Matches(classAnalyzedModel.methodStr[j], @"(?:public\s|private\s|protected\s|internal\s|)?[\s\w<>]*\s+(?<methodName>\w+)\s*\(\s*(?:(ref\s|in\s|out\s)?\s*(?<parameterType>[\w<>\?\[\]]+)?\s+(?<parameter>\w+)?\s*,?\s*)+\)"))
                {
                    methodInfo.methodName = match.Groups["methodName"].Value;
                    methodInfo.isReturningValue = !classAnalyzedModel.methodStr[j].Contains("void ");
                    methodInfo.typeParameterPair = new List<ClassAnalyzedModelMethodInfoDetails>();

                    int i = 0;
                    foreach (var capture in match.Groups["parameterType"].Captures)
                    {
                        var classAnalyzedModelMethodInfoDetails = new ClassAnalyzedModelMethodInfoDetails();
                        classAnalyzedModelMethodInfoDetails.key = match.Groups["parameterType"].Captures[i].Value;
                        classAnalyzedModelMethodInfoDetails.value = match.Groups["parameter"].Captures[i].Value;
                        i++;
                        methodInfo.typeParameterPair.Add(classAnalyzedModelMethodInfoDetails);
                    }
                }
                classAnalyzedModel.methodListObj.Add(methodInfo);
            }

            return classAnalyzedModel;
        }
    }
}