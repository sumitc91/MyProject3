using BT.SaaS.Core.UI.Framework.ServiceEntities;
using BT.SaaS.HD.Mobility.Modules;
using BT.SaaS.HD.Mobility.Modules.SimReplacement.Configuration.Implementation;
using BT.SaaS.HD.Mobility.Modules.SimReplacement.Configuration.Interfaces;
using BT.SaaS.HD.Mobility.ViewModels.SimReplacement;

namespace BT.SaaS.HD.Mobility.Controllers.SimReplacement
{
    using System.Web.Mvc;
    using UI.Common.ActionFilters;
    using UI.Common.ModuleHandlerFactory;
    using UI.Common.Controllers;
    using BT.SaaS.HD.Mobility.Modules.ContextBuilder.Configuration;
    using BT.SaaS.HD.Mobility.Utility;


    public class SimReplacementController : ApplicationController
    {

        private readonly ISimReplacementInvoker _simReplacementInvoker;
        private ISimModuleHandler _simTypeModuleHandler;
        private ISimReplacementWaivingChargeModuleHandler _simReplacementWaivingChargeModuleHandler;

        public SimReplacementController()
            : this(ModuleHandlerAggregator.Current)
        {
        }
        
        public SimReplacementController(ModuleHandlerAggregator moduleHandlerAggregator)
        {
            this.ModuleHandlerAggregator = moduleHandlerAggregator;

            this.ModuleHandlerAggregator.RegisterType<SimReplacementInvoker>();
            this.ModuleHandlerAggregator.RegisterType<SimTypeModuleHandler> ();
            this.ModuleHandlerAggregator.RegisterType<SimReplacementWaivingChargeModuleHandler>();
            this.ModuleHandlerAggregator.Register<Modules.Modify.ResetBasketHandler>();
            this.ModuleHandlerAggregator.Register<SimTypeModuleHandler>();
            this.ModuleHandlerAggregator.RegisterType<ConfigurationContextBuilder>();
            this.ModuleHandlerAggregator.Register<Modules.Modify.SimTypeModuleHandler>();

            _simReplacementInvoker = ModuleHandlerAggregator.TryCreateOrGet<ISimReplacementInvoker>();
        }


        public ModuleHandlerAggregator ModuleHandlerAggregator { get; set; }
        
        [LayoutInit("_Footer")]
        public ActionResult Index(bool isReadOnly)
        {
            return View(_simReplacementInvoker.GetSimReplacementViewModel(isReadOnly));   
        }

        [HttpPost]
        public JsonResult AddProductForSimType(string productCode, string parentProductCode)
        {
            _simTypeModuleHandler = ModuleHandlerAggregator.TryCreateOrGet<ISimModuleHandler>();            
            var simTypeViewModel = _simReplacementInvoker.AddSimTypeProduct(productCode, parentProductCode);

            return Json(ModuleHandlerAggregator.AjaxResponse(_simTypeModuleHandler, em => simTypeViewModel));
        }

        
        public JsonResult WaiveSimReplacementCharge(string chargeTypeProductCode, bool isSimReplacementChargedWaived)
        {
            _simReplacementWaivingChargeModuleHandler = ModuleHandlerAggregator.TryCreateOrGet<ISimReplacementWaivingChargeModuleHandler>();
            _simReplacementInvoker.UpdateWaiveAttributeOfSimReplacementChargeProduct(chargeTypeProductCode,
                isSimReplacementChargedWaived);
            return default(JsonResult);
        }

        [HttpPost]
        public ContentResult Update(SimReplacementViewModel simReplacementViewModel)
        {
            _simReplacementInvoker.Update(simReplacementViewModel);
            return Content(_simReplacementInvoker.Navigate(NavigationStepsEnum.DeliveryDetails), RedirectionType.Url.ToString());
        }
        public ContentResult ReloadSimReplacement()
        {
            var context = ModuleHandlerAggregator.TryCreateOrGet<ConfigurationContextBuilder>().GetProductsViewModelContext();
            var resetServiceHandler = ModuleHandlerAggregator.TryCreateOrGet<Modules.Modify.IResetBasketHandler>();
            var SimTypeModuleHandler = ModuleHandlerAggregator.TryCreateOrGet<Modules.Modify.ISimTypeModuleHandler>();
            resetServiceHandler.MergeSessionBasket();
            resetServiceHandler.SetJourneyScenario();
            return Content(SimTypeModuleHandler.GenerateUrlForSimType(context), RedirectionType.Url.ToString());
        }

        public ContentResult Cancel()
        {
            _simReplacementInvoker.Cancel();
            return Content(_simReplacementInvoker.Navigate(NavigationStepsEnum.ModifyConfiguration), RedirectionType.Url.ToString());
        }  

    }
}
