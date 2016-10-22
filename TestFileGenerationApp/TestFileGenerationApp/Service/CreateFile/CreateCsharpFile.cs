using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TestFileGenerationApp.Models;

namespace TestFileGenerationApp.Service.CreateFile
{
    public class CreateCsharpFile : ICreateFile
    {
        public string execute(ClassAnalyzedModel classAnalyzedModel, HttpServerUtilityBase Server)
        {
            string path = Path.Combine(Server.MapPath("~/TestUploads/"), classAnalyzedModel.classNameStr + "Tests.cs");
            if (!File.Exists(path))
            {
                FileStream fs = new FileStream(path, FileMode.CreateNew);
                fs.Close();
            }

            TextWriter tw = new StreamWriter(path);
            BuildCsTestFileWithMetaData(classAnalyzedModel, tw);
            return path;
        }

        public static void BuildCsTestFileWithMetaData(ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            //write namespaces
            addNamespacesForTestingFramework(classAnalyzedModel, tw);
            openClassNamespace(classAnalyzedModel, tw);
            /**/
            openClassClass(classAnalyzedModel, tw);
            /**//**/
            declareGlobalVariables(classAnalyzedModel, tw);
            /**//**/
            declareClassSetup(classAnalyzedModel, tw);
            /**//**/
            foreach (var method in classAnalyzedModel.methodListObj)
            /**//**/
            {
                /**//**/
                declareClassMethod(method, classAnalyzedModel, tw);
                /**//**/
            }
            /**/
            closeClassClass(classAnalyzedModel, tw);
            closeClassNamespace(classAnalyzedModel, tw);
            tw.Close();
        }
        private static void declareClassMethod(ClassAnalyzedModelMethodInfo method, ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            if (string.IsNullOrEmpty(method.methodName) || method.methodName == classAnalyzedModel.classNameStr)
                return;

            addEmptyLineInClass(tw);
            addEmptyLineInClass(tw);
            //addAnnotationInClass(tab(2) + "[TestMethod, NUnitFw.Test]", tw);
            addAnnotationInClass(tab(2) + "[NUnitFw.Test]", tw);
            tw.WriteLine(tab(2) + "public void " + method.methodName + "_Test"+"()");
            tw.WriteLine(tab(2) + "{");

            string inputParams = "";
            addEmptyLineInClass(tw);
            foreach (var item in method.typeParameterPair)
            {
                if (item.key == "string")
                {
                    tw.WriteLine(tab(3) + "var " + item.value + " = \"\";");
                }
                else
                {
                    tw.WriteLine(tab(3) + "var " + item.value + " = new " + item.key + "();");
                }
                inputParams += item.value + ",";
            }

            if (!string.IsNullOrEmpty(inputParams))
            {
                inputParams = inputParams.TrimEnd(',');
            }

            tw.WriteLine(tab(3) + "//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);");
            if (method.isReturningValue)
            {
                tw.WriteLine(tab(3) + "var response = _" + Char.ToLowerInvariant(classAnalyzedModel.classNameStr[0]) + classAnalyzedModel.classNameStr.Substring(1) + ".Object." + method.methodName + "(" + inputParams + ");");
                tw.WriteLine(tab(3) + "_" + Char.ToLowerInvariant(classAnalyzedModel.classNameStr[0]) + classAnalyzedModel.classNameStr.Substring(1)+".VerifyAll();");
                tw.WriteLine(tab(3) + "//NUnitFw.Assert.AreEqual(\"compareFrom\", \"compareTo\");");
            }
            else
            {
                tw.WriteLine(tab(3) + "_" + Char.ToLowerInvariant(classAnalyzedModel.classNameStr[0]) + classAnalyzedModel.classNameStr.Substring(1) + ".Object." + method.methodName + "(" + inputParams + ");");
                tw.WriteLine(tab(3) + "_" + Char.ToLowerInvariant(classAnalyzedModel.classNameStr[0]) + classAnalyzedModel.classNameStr.Substring(1) + ".VerifyAll();");
            }

            tw.WriteLine(tab(2) + "}");
        }
        private static void declareClassSetup(ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            //addAnnotationInClass(tab(2) + "[TestInitialize, NUnitFw.SetUp]", tw);
            addAnnotationInClass(tab(2) + "[NUnitFw.SetUp]", tw);
            tw.WriteLine(tab(2) + "public void Setup()");
            tw.WriteLine(tab(2) + "{");
            instantiateGlobalVariables(classAnalyzedModel, tw);
            addEmptyLineInClass(tw);
            setupMockGlobalVariables(classAnalyzedModel, tw);
            instantiateGivenClassVariable(classAnalyzedModel, tw);
            tw.WriteLine(tab(2) + "}");
        }
        private static void setupMockGlobalVariables(ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            //instantiate Global variables.
            foreach (var item in classAnalyzedModel.instancesStr)
            {
                if (item.Contains("<"))
                {
                    tw.WriteLine(tab(3) + "_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<" + item + ">()).Returns(_" + GetVariableNameForGenericModuleHandler(item) + ".Object);");
                }
                else
                {
                    if (item[0] == 'I')
                    {
                        tw.WriteLine(tab(3) + "_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<" + item + ">()).Returns(_" + (Char.ToLowerInvariant(item[1]) + item.Substring(2)).Replace(".", "_") + ".Object);");
                    }
                    else
                    {
                        tw.WriteLine(tab(3) + "_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<" + item + ">()).Returns(_" + (Char.ToLowerInvariant(item[0]) + item.Substring(1)).Replace(".", "_") + ".Object);");
                    }
                }

            }
        }
        private static void instantiateGlobalVariables(ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            instantiateExtraGlobalVariables(classAnalyzedModel, tw);
            //instantiate Global variables.
            foreach (var item in classAnalyzedModel.instancesStr)
            {
                if (item.Contains("<"))
                {
                    tw.WriteLine(tab(3) + "_" + GetVariableNameForGenericModuleHandler(item) + " = new Mock<" + item + ">();");
                }
                else
                {
                    if (item[0] == 'I')
                    {
                        tw.WriteLine(tab(3) + "_" + (Char.ToLowerInvariant(item[1]) + item.Substring(2)).Replace(".","_") + " = new Mock<" + item + ">();");
                    }
                    else
                    {
                        tw.WriteLine(tab(3) + "_" + (Char.ToLowerInvariant(item[0]) + item.Substring(1)).Replace(".", "_") + " = new Mock<" + item + ">();");
                    }
                }
            }
        }

        private static string GetVariableNameForGenericModuleHandler(string item)
        {
            return (Char.ToLowerInvariant(item[1]) + item.Split('<')[0].Substring(2) + "" + item.Split('<')[1].Split(',')[0]).Replace(".", "_");
        }
        private static void declareGlobalVariables(ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            //declare variables.
            declareExtraGlobalVariables(classAnalyzedModel, tw);
            foreach (var item in classAnalyzedModel.instancesStr)
            {
                if (item.Contains("<"))
                {
                    tw.WriteLine(tab(2) + "private Mock<" + item + "> _" + GetVariableNameForGenericModuleHandler(item) + ";");
                }
                else
                {
                    if (item[0] == 'I')
                    {
                        tw.WriteLine(tab(2) + "private Mock<" + item + "> _" + Char.ToLowerInvariant(item[1]) + item.Substring(2).Replace(".", "_") + ";");
                    }
                    else
                    {
                        tw.WriteLine(tab(2) + "private Mock<" + item + "> _" + Char.ToLowerInvariant(item[0]) + item.Substring(1).Replace(".", "_") + ";");
                    }
                }
            }

            addEmptyLineInClass(tw);
        }
        private static void addNamespacesForTestingFramework(ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            foreach (var usedNamespaceStr in classAnalyzedModel.usedNamespacesStr)
            {
                tw.WriteLine(usedNamespaceStr.TrimStart());
            }

            //adding current class namespace
            tw.WriteLine("using "+classAnalyzedModel.classNamespaceStr.TrimStart()+";");

            addAdditionalNamespacesForTestingFramework(tw);
            addEmptyLineInClass(tw);
        }
        private static void addEmptyLineInClass(TextWriter tw)
        {
            tw.WriteLine();
        }
        private static void addAnnotationInClass(string annotationStr, TextWriter tw)
        {
            tw.WriteLine(annotationStr);
        }
        private static string tab(int count)
        {
            string str = "";
            for (int i = 0; i < count; i++)
            {
                str += "\t";
            }
            return str;
        }
        private static void openClassNamespace(ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            tw.WriteLine("namespace " + classAnalyzedModel.classNamespaceStr.Replace("Mobility", "Mobility.Tests"));
            tw.WriteLine("{");
        }
        private static void openClassClass(ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            //addAnnotationInClass(tab(1) + "[TestClass(), NUnitFw.TestFixture]", tw);
            addAnnotationInClass(tab(1) + "[NUnitFw.TestFixture]", tw);
            tw.WriteLine(tab(1) + "public class " + classAnalyzedModel.classNameStr + "Test");
            tw.WriteLine(tab(1) + "{");
        }
        private static void closeClassNamespace(ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            tw.WriteLine("}");
        }
        private static void closeClassClass(ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            tw.WriteLine(tab(1) + "}");
        }
        private static void addAdditionalNamespacesForTestingFramework(TextWriter tw)
        {
            //tw.WriteLine("using Microsoft.VisualStudio.TestTools.UnitTesting;");
            tw.WriteLine("using Moq;");
            tw.WriteLine("using NUnitFw = NUnit.Framework;");
        }
        private static void declareExtraGlobalVariables(ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            tw.WriteLine(tab(2) + "private Mock<ModuleHandlerAggregator> _moduleHandlerAggregator;");
            tw.WriteLine(tab(2) + "private Mock<" + classAnalyzedModel.classNameStr + "> _" + Char.ToLowerInvariant(classAnalyzedModel.classNameStr[0]) + classAnalyzedModel.classNameStr.Substring(1) + ";");
        }
        private static void instantiateExtraGlobalVariables(ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            tw.WriteLine(tab(3) + "_moduleHandlerAggregator = new Mock<ModuleHandlerAggregator>();");
        }
        private static void instantiateGivenClassVariable(ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            addEmptyLineInClass(tw);
            tw.WriteLine(tab(3) + "_" + Char.ToLowerInvariant(classAnalyzedModel.classNameStr[0]) + classAnalyzedModel.classNameStr.Substring(1) + " = new Mock<" + classAnalyzedModel.classNameStr + ">(_moduleHandlerAggregator.Object){ CallBase = true };");
        }
    }
}