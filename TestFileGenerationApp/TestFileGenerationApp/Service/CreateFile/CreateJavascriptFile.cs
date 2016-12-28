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
            
            addEmptyLineInClass(tw);
            addEmptyLineInClass(tw);
            
            tw.WriteLine(tab(1) + "it(\"should test "+ method + " function\", function() {");
            tw.WriteLine(tab(2) + "//scope.pageResetConfirmationPopupModel.showPopup = false;");
            tw.WriteLine(tab(2) + "scope." + method + "();");
            tw.WriteLine(tab(2) + "//expect(scope.pageResetConfirmationPopupModel.showPopup).toEqual(true);");
            
            tw.WriteLine(tab(1) + "});");
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
        
        private static void declareExtraGlobalVariables(ClassAnalyzedModel classAnalyzedModel, TextWriter tw)
        {
            tw.WriteLine(tab(1) + "var rootScope;");
            tw.WriteLine(tab(1) + "var scope;");
            tw.WriteLine(tab(1) + "var moduleHandleAggregrator;");
            tw.WriteLine(tab(1) + "var q;");
            tw.WriteLine(tab(1) + "var deffered;");
            tw.WriteLine(tab(1) + "var cacheFactory;");
        }
        
    }
}