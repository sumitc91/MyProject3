using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestFileGenerationApp.Models
{
    public class ClassAnalyzedModel
    {
        public List<string> usedNamespacesStr { get; set; }
        public string classNamespaceStr { get; set; }
        public string classNameStr { get; set; }
        public List<string> instancesStr { get; set; }

        public List<string> methodStr { get; set; }
        public List<ClassAnalyzedModelMethodInfo> methodListObj { get; set; }
    }

    public class ClassAnalyzedModelMethodInfo
    {
        public string methodName { get; set; }
        public bool isReturningValue { get; set; }
        public List<ClassAnalyzedModelMethodInfoDetails> typeParameterPair { get; set; }
    }

    public class ClassAnalyzedModelMethodInfoDetails
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}