using System.Linq;
using BT.SaaS.Core.Entity.Utils.Facades;
using BT.SaaS.Core.Shared.Entities;
using BT.SaaS.Core.UI.Framework.ServiceEntities;
using BT.SaaS.HD.Mobility.ContextBuilder.SimReplacement;
using BT.SaaS.HD.Mobility.Helper;
using BT.SaaS.HD.Mobility.Helper.Interfaces;
using BT.SaaS.HD.Mobility.Modules.ContextBuilder.Configuration;
using BT.SaaS.HD.Mobility.Modules.Modify;
using BT.SaaS.HD.Mobility.Modules.SimReplacement.Configuration.Interfaces;
using BT.SaaS.HD.Mobility.Modules.SimReplacement.Processor;
using BT.SaaS.HD.Mobility.Utility;
using BT.SaaS.HD.Mobility.ViewModels;
using BT.SaaS.HD.Mobility.ViewModels.SimReplacement;
using BT.SaaS.HD.UI.Common.ModuleHandlerFactory;
using BT.SaaS.HD.UI.Common.Modules;
using BT.SaaS.HD.UI.Common.Modules.Configuration;
using BT.SaaS.HD.UI.Common.ViewModelContext;
using BT.SaaS.HD.UI.Common.ViewModels;
using BT.SaaS.HD.Mobility.Mapper.Interface.SimReplacement;
using BT.SaaS.HD.UI.Common.Configuration.ViewModelContext;

namespace BT.SaaS.HD.Mobility.Modules.SimReplacement.Configuration.Implementation
{
    public class SimReplacementInvoker : ISimReplacementInvoker
    {
        private readonly ModuleHandlerAggregator _aggregator;
        private IBroadcastModuleHandler<FooterModuleViewModel, FooterModuleContext> _footerModuleHandler;
        private ConfigurationContextBuilder _configurationContextBuilder;
        private ISimActivationStatusModuleHandler _simActivationStatusModuleHandler;
        private ISimReplacementReasonsModuleHandler _simReplacementReasonsModuleHandler;
        private ISimReplacementWaivingChargeModuleHandler _simReplacementWaivingchargeReasonsModuleHandler;
        private IModuleHandler<SimTypeViewModel, ProductsViewModelContext> _replacementSimTypeModuleHandler;
        private ISimReplacementContextBuilder _simReplacementConfigurationBuilder;
        private ISimModuleHandler _simModuleHandler;
        private ISessionFacade _sessionFacade;
        private IBasketFacade _basketFacade;
        private IResetBasketHandler _resetBasketHandler;

        public SimReplacementInvoker(): this(ModuleHandlerAggregator.Current){}

        public SimReplacementInvoker(ModuleHandlerAggregator aggregator)
        {
            //Register
            _aggregator = aggregator;
            _aggregator.RegisterType<SimReplacementReasonsModuleHandler>();
            _aggregator.RegisterType<SimReplacementWaivingChargeModuleHandler>();
            _aggregator.RegisterType<SimReplacementContextBuilder>();
            _aggregator.RegisterType<ConfigurationContextBuilder>();
            _aggregator.Register<SimTypeModuleHandler>();
            _aggregator.Register<ProceedToCheckoutModuleHandler>();
            _aggregator.RegisterType<SessionFacade>();
            _aggregator.RegisterType<SimActivationStatusModuleHandler>();
            _aggregator.Register<ContractLengthModuleHandler>();
            _aggregator.Register<FooterModuleHandler>();
            _aggregator.RegisterType<L2CAssetProcessor>();
            _aggregator.RegisterType<T2RAssetProcessor>();
            _aggregator.RegisterType<SimHelper>();
            _aggregator.RegisterType<BasketFacade>();
            _aggregator.RegisterType<PortalNavigationHelper>();
            _aggregator.RegisterType<ResetBasketHandler>();
            _aggregator.RegisterType<ReplacementSimTypeModuleHandler>();
        }


        public SimTypeViewModel AddSimTypeProduct(string productCode, string parentProductCode)
        {
            #region Creating Instances

            _simModuleHandler = _aggregator.TryCreateOrGet<ISimModuleHandler>();
            _configurationContextBuilder = _aggregator.TryCreateOrGet<ConfigurationContextBuilder>();

            #endregion
            
            _simModuleHandler.AddProduct(productCode, parentProductCode);
            _simModuleHandler.AddChargeTypeProductForReplacement(productCode);
            _simModuleHandler.SetNoneForExistingSimTypes();
            _simModuleHandler.UpdateRPIForSimTypeProduct();
            _simModuleHandler.UpdateRPIForChargeTypeProduct();
            _simModuleHandler.AddDummyDeliveryDetails();
            return _simModuleHandler.Initialize(_configurationContextBuilder.GetProductsViewModelContext());
        }

        public SimReplacementViewModel GetSimReplacementViewModel(bool isReadOnly)
        {
            #region Creating Instances

            _resetBasketHandler = _aggregator.TryCreateOrGet<IResetBasketHandler>();
            _simReplacementConfigurationBuilder = _aggregator.TryCreateOrGet<ISimReplacementContextBuilder>();
            _simModuleHandler = _aggregator.TryCreateOrGet<ISimModuleHandler>();
            _simActivationStatusModuleHandler = _aggregator.TryCreateOrGet<ISimActivationStatusModuleHandler>();
            _simReplacementReasonsModuleHandler = _aggregator.TryCreateOrGet<ISimReplacementReasonsModuleHandler>();
            _simReplacementWaivingchargeReasonsModuleHandler =
                _aggregator.TryCreateOrGet<ISimReplacementWaivingChargeModuleHandler>();
            _footerModuleHandler =
                _aggregator.TryCreateOrGet<IBroadcastModuleHandler<FooterModuleViewModel, FooterModuleContext>>();
            _configurationContextBuilder = _aggregator.TryCreateOrGet<ConfigurationContextBuilder>();

            #endregion
            
            var source = GetSource();
            ProcessAssetBasedOnSource(source);
            SetBasketScenario(source);
            _resetBasketHandler.SetJourneyScenario();
            var simReplacementViewModelContext = _simReplacementConfigurationBuilder.GetSimReplacementViewModelContext();

            return new SimReplacementViewModel()
            {
                SimTypes = _simModuleHandler.Initialize(_configurationContextBuilder.GetProductsViewModelContext()),
                SimActivationStatus = _simActivationStatusModuleHandler.Initialize(simReplacementViewModelContext),
                ReasonForReplacingSim = _simReplacementReasonsModuleHandler.Initialize(simReplacementViewModelContext),
                ReasonForWaivingCharge = _simReplacementWaivingchargeReasonsModuleHandler.Initialize(simReplacementViewModelContext),
                FooterViewModel = _footerModuleHandler.Initialize(_configurationContextBuilder.GetFooterViewModelContext()),
                IsReadOnly = isReadOnly
            };
        }

        internal virtual void ProcessAssetBasedOnSource(string source)
        {
            var simReplacementAssetProcessor = GetInstanceOfSource(source);
            simReplacementAssetProcessor.ProcessAsset();
        }

        internal virtual string GetSource()
        {
            _sessionFacade = _aggregator.TryCreateOrGet<ISessionFacade>();

            return _sessionFacade.JourneyData.RequestType;
        }

        internal virtual void SetBasketScenario(string source)
        {
            _basketFacade = _aggregator.TryCreateOrGet<ISessionFacade>().Basket;

            var basket = _basketFacade.GetBasket();
            switch (source)
            {
                case Constants.T2R:
                    {
                        basket.Scenario = ScenarioEnum.T2RMobileSIMReplace;
                        break;
                    }
                default:
                    {
                        basket.Scenario = ScenarioEnum.L2CMobileSIMReplace;
                        break;
                    }
            }
        }


        internal virtual SimReplacementAssetProcessor GetInstanceOfSource(string source)
        {
            switch (source)
            {
                case Constants.T2R:
                    return _aggregator.TryCreateOrGet<T2RAssetProcessor>();
                default:
                    return _aggregator.TryCreateOrGet<L2CAssetProcessor>();
            }
        }


        public WaiveChargeReasonsViewModel UpdateWaiveAttributeOfSimReplacementChargeProduct(string chargeTypeProductCode, bool isSimReplacementChargedWaived)
        {
            #region Creating Instances

            _simReplacementConfigurationBuilder = _aggregator.TryCreateOrGet<ISimReplacementContextBuilder>();
            _simReplacementWaivingchargeReasonsModuleHandler =
                _aggregator.TryCreateOrGet<ISimReplacementWaivingChargeModuleHandler>();

            #endregion

            var simReplacementViewModelContext = _simReplacementConfigurationBuilder.GetSimReplacementViewModelContext();
            _simReplacementWaivingchargeReasonsModuleHandler.WaiveSimReplacementCharge(chargeTypeProductCode, isSimReplacementChargedWaived);
            return _simReplacementWaivingchargeReasonsModuleHandler.Initialize(simReplacementViewModelContext);
        }

        public virtual void Update(SimReplacementViewModel simReplacementViewModel)
        {
            #region Creating Instances

            _replacementSimTypeModuleHandler = this._aggregator.TryCreateOrGet<IModuleHandler<SimTypeViewModel, ProductsViewModelContext>>();
            _simReplacementWaivingchargeReasonsModuleHandler =
                _aggregator.TryCreateOrGet<ISimReplacementWaivingChargeModuleHandler>();
            _simActivationStatusModuleHandler = _aggregator.TryCreateOrGet<ISimActivationStatusModuleHandler>();
            _simReplacementReasonsModuleHandler = _aggregator.TryCreateOrGet<ISimReplacementReasonsModuleHandler>();

            #endregion

            _simActivationStatusModuleHandler.Update(simReplacementViewModel.SimActivationStatus, null);
            _simReplacementReasonsModuleHandler.Update(simReplacementViewModel.ReasonForReplacingSim, null);
            _simReplacementWaivingchargeReasonsModuleHandler.Update(simReplacementViewModel.ReasonForWaivingCharge, null);
            _replacementSimTypeModuleHandler.Update(simReplacementViewModel.SimTypes, null);
        }

        public virtual string Navigate()
        {
            _sessionFacade = _aggregator.TryCreateOrGet<ISessionFacade>();

            _sessionFacade.JourneyNavigator.SetCurentNavigation(NavigationStepsEnum.DeliveryDetails);
            _sessionFacade.JourneyNavigator.UpdateOrderItemNavigator("Replacement",_sessionFacade.JourneyNavigator.GetOrderItemNavigatorList().First());
            return _aggregator.TryCreateOrGet<IPortalNavigationHelper>()
                .GetNavigationUrl(NavigationStepsEnum.DeliveryDetails);
        }
    }
}