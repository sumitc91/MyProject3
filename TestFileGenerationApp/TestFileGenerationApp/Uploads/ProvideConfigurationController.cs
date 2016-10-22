using System.Web.Mvc;
using BT.SaaS.HD.L2CJourney.ViewModels.ConfigurationPage.MaintenanceModule;
using BT.SaaS.HD.Mobility.Modules;
using BT.SaaS.HD.Mobility.Modules.Context;
using BT.SaaS.HD.Mobility.Modules.ContextBuilder.Configuration;
using BT.SaaS.HD.Mobility.ViewModels;
using BT.SaaS.HD.UI.Common.ActionFilters;
using BT.SaaS.HD.UI.Common.Configuration.ViewModelContext;
using BT.SaaS.HD.UI.Common.Configuration.ViewModels;
using BT.SaaS.HD.UI.Common.ContextBuilder;
using BT.SaaS.HD.UI.Common.ModuleHandlerFactory;
using BT.SaaS.HD.UI.Common.Modules;
using BT.SaaS.HD.UI.Common.ViewModelContext;
using BT.SaaS.HD.UI.Common.ViewModels;
using BT.SaaS.HD.Mobility.Controllers.Provide;

namespace BT.SaaS.HD.Mobility.Controllers
{
    using BT.SaaS.HD.Mobility.Modules.Configuration;
    using BT.SaaS.HD.Mobility.Modules.Configuration.BTWifi.NotNeeded;
    using BT.SaaS.HD.Mobility.ViewModels.Provide.Accessory;
    using BT.SaaS.HD.Mobility.Controllers.Provide.Helper;

    public class ProvideConfigurationController : ConfigurationBaseController
    {
        IModuleHandler<PoiProductsViewModel, PoiProductsViewModelContext> prodDetailsModuleHandler;
        IModuleHandler<ContractLengthViewModel, ProductsViewModelContext> contractLengthModuleHandler;
        IModuleHandler<ExtrasViewModel, ProductsViewModelContext> extrasModuleHandler;
        IModuleHandler<SimTypeViewModel, ProductsViewModelContext> simTypeModuleHandler;
        IBroadcastModuleHandler<ChargesViewModel, MPBSummaryViewModelContext> totalChargesModuleHandler;
        IModuleHandler<PricePlanViewModel, ProductsViewModelContext> pricePlanModuleHandler;
        ConfigurationContextBuilder configurationBuilder;
        IModuleHandler<AccessoryListViewModel, ProductsViewModelContext> accessoriesModuleHandler;

        public ProvideConfigurationController()
            : this(ModuleHandlerAggregator.Current)
        {
        }

        public ProvideConfigurationController(ModuleHandlerAggregator aggregator)
            : base(aggregator)
        {
            aggregator.Register<PoiProductsModuleHandler>()
                .Register<ContractLengthModuleHandler>()
                .Register<SimTypeModuleHandler>()
                .Register<PricePlanModuleHandler>()
                .Register<AccessoriesModuleHandler>()
                .Register<PricePlanModuleHandler>()             
                .RegisterType<UpdateModuleHandler>();

            ConfigurationInfo infoObj = new ConfigurationInfo();
            string controller = "SIMO";
            string MHList;

            switch (controller)
            {
                case "SIMO":
                    MHList = infoObj.GetValue("SIMO");
                    break;
                case "SOLO":
                    MHList = infoObj.GetValue("SOLO");
                    break;
                default:
                    MHList = infoObj.GetValue("MBB");
                    break;
            }

             prodDetailsModuleHandler = Aggregator.TryCreateOrGet<IModuleHandler<PoiProductsViewModel, PoiProductsViewModelContext>>();
             contractLengthModuleHandler = Aggregator.TryCreateOrGet<IModuleHandler<ContractLengthViewModel, ProductsViewModelContext>>();
             extrasModuleHandler = Aggregator.TryCreateOrGet<IModuleHandler<ExtrasViewModel, ProductsViewModelContext>>();
             simTypeModuleHandler = Aggregator.TryCreateOrGet<IModuleHandler<SimTypeViewModel, ProductsViewModelContext>>();
             totalChargesModuleHandler = Aggregator.TryCreateOrGet<IBroadcastModuleHandler<ChargesViewModel, MPBSummaryViewModelContext>>();
             pricePlanModuleHandler = Aggregator.TryCreateOrGet<IModuleHandler<PricePlanViewModel, ProductsViewModelContext>>();
             configurationBuilder = Aggregator.TryCreateOrGet<ConfigurationContextBuilder>();
             accessoriesModuleHandler = Aggregator.TryCreateOrGet<IModuleHandler<AccessoryListViewModel, ProductsViewModelContext>>();


        }

        [LayoutInit]
        public ActionResult Index()
        {

           
            var productsViewModelContext = configurationBuilder.GetProductsViewModelContext();

                var provideSimoConfigurationViewModel = new ProvideSIMOConfigurationViewModel
                {
                    ProductDetails = prodDetailsModuleHandler.Initialize(configurationBuilder.GetPoiProductsViewModelContext()),
                    ContractLength = contractLengthModuleHandler.Initialize(productsViewModelContext),
                    SimType = simTypeModuleHandler.Initialize(productsViewModelContext),
                    Extras = extrasModuleHandler.Initialize(productsViewModelContext),
                    ChargesViewModel = totalChargesModuleHandler.Initialize(configurationBuilder.GetMpbSummaryViewModelContext()),
                    PricePlan = pricePlanModuleHandler.Initialize(configurationBuilder.GetProductsViewModelContext()),
                };
                return base.Index(provideSimoConfigurationViewModel);           

        }

        [HttpPost]
        public ActionResult Previous(ProvideSIMOConfigurationViewModel viewModel)
        {

            return base.Navigate(true);
        }

        [HttpPost]
        public ActionResult Next(ProvideSIMOConfigurationViewModel viewModel)
        {
            var footerModuleHandler = Aggregator.TryCreateOrGet<IBroadcastModuleHandler<FooterModuleViewModel, FooterModuleContext>>();

            var configurationMasterBuilder = Aggregator.TryCreateOrGet<ConfigurationModuleContextBuilder>();
            footerModuleHandler.Update(viewModel.FooterViewModel, configurationMasterBuilder.GetFooterViewModelContext());

            return this.Navigate();
        }
    }
}