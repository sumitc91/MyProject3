using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TestFileGenerationApp.Models;

namespace TestFileGenerationApp.Service.CreateFile
{
    public class CreateJavascriptFile : ICreateFile
    {
        public string execute(ClassAnalyzedModel classAnalyzedModel, HttpServerUtilityBase Server)
        {
            string path = Path.Combine(Server.MapPath("~/TestUploads/"), classAnalyzedModel.classNameStr + "Tests.js");
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
            
            /**//**/
            declareGlobalVariables(classAnalyzedModel, tw);
            /**//**/
            declareBeforeEachSetup(classAnalyzedModel, tw);
            /**//**/
            foreach (var method in classAnalyzedModel.methodStr)
            /**//**/
            {
                /**//**/
                declareClassMethod(method, classAnalyzedModel, tw);
                /**//**/
            }
            /**/
            //closeClassClass(classAnalyzedModel, tw);
            closeClassNamespace(classAnalyzedModel, tw);
            tw.Close();
        }
        private static void declareClassMethod(String method, ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            //it("should test showPageResetConfirmationPopup function", function() {

            //    scope.pageResetConfirmationPopupModel.showPopup = false;
            //    scope.showPageResetConfirmationPopup();
            //    expect(scope.pageResetConfirmationPopupModel.showPopup).toEqual(true);
            //});

            addEmptyLineInClass(tw);
            addEmptyLineInClass(tw);
            
            tw.WriteLine(tab(2) + "it(\"should test "+ method + " function\", function() {");
            tw.WriteLine(tab(3) + "//scope.pageResetConfirmationPopupModel.showPopup = false;");
            tw.WriteLine(tab(3) + "scope." + method + "();");
            tw.WriteLine(tab(3) + "//expect(scope.pageResetConfirmationPopupModel.showPopup).toEqual(true);");
            
            tw.WriteLine(tab(2) + "});");
        }
        private static void declareBeforeEachSetup(ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            
            tw.WriteLine(tab(1) + "beforeEach(function() {");
            tw.WriteLine(tab(2) + "module(\"pageApp\");");
            tw.WriteLine(tab(2) + "inject(function($injector) {");
            tw.WriteLine(tab(3) + "rootScope = $injector.get(\"$rootScope\");", tw);
            tw.WriteLine(tab(3) + "scope = rootScope.$new();");
            tw.WriteLine(tab(3) + "q = $injector.get(\"$q\");");
            tw.WriteLine(tab(3) + "cacheFactory = $injector.get(\"CacheFactory\");", tw);
            tw.WriteLine(tab(3) + "deffered = q.defer();");
            tw.WriteLine(tab(3) + "moduleHandleAggregrator = $injector.get(\"moduleAggregator\");");
            tw.WriteLine(tab(3) + "$injector.get(\"$controller\")(\""+ classAnalyzedModel.classNameStr + "\", { $scope: scope, $rootScope: rootScope,  cacheFactory: cacheFactory, moduleAggregator: moduleHandleAggregrator });");
            tw.WriteLine(tab(2) + "});");
            tw.WriteLine(tab(1) + "});");
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
            
            addEmptyLineInClass(tw);
        }
        private static void addNamespacesForTestingFramework(ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            
            tw.WriteLine("/// <reference path=\"../../../Scripts/jquery-1.12.0.min.js\" />");
            tw.WriteLine("/// <reference path=\"../../../Scripts/angular-mocks-1.2.21.js\" />");
            tw.WriteLine("/// <reference path=\"../../../Scripts/jasmine.js\" />");
            tw.WriteLine("/// <reference path=\"../../SpyOnServicesHelper.js\" />");
            tw.WriteLine("/// <reference path=\"../../../../bt.saas.hd.mobility/js/mobilityscript.js\" />");
            tw.WriteLine("/// <reference path=\"../../../../bt.saas.hd.mobility/js/modules/moduleaggregator.js\" />");
            tw.WriteLine("/// <reference path=\"../../../../bt.saas.hd.mobility/js/services/cachefactory.js");


            tw.WriteLine("/// <reference path=\"Add Path of Js File\" />");
            tw.WriteLine("/// <reference path=\"Add Path of any dependency\" />");
            
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
            tw.WriteLine("describe(\"js_"+ classAnalyzedModel.classNameStr+ "_test: \", function ()");
            tw.WriteLine("{");
        }
        
        private static void closeClassNamespace(ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            tw.WriteLine("}");
        }
        private static void closeClassClass(ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            tw.WriteLine(tab(1) + "}");
        }
        
        private static void declareExtraGlobalVariables(ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            tw.WriteLine(tab(1) + "var rootScope;");
            tw.WriteLine(tab(1) + "var scope;");
            tw.WriteLine(tab(1) + "var moduleHandleAggregrator;");
            tw.WriteLine(tab(1) + "var q;");
            tw.WriteLine(tab(1) + "var deffered;");
            tw.WriteLine(tab(1) + "var cacheFactory;");
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