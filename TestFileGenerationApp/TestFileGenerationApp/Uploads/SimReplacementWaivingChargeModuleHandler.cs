using System;
using System.Collections.Generic;
using System.Linq;
using BT.SaaS.Core.Entity.Utils.Extensions;
using BT.SaaS.Core.Entity.Utils.Facades;
using BT.SaaS.Core.Shared.Entities;
using BT.SaaS.HD.Mobility.Helper;
using BT.SaaS.HD.Mobility.Mapper.Implementations.Shared;
using BT.SaaS.HD.Mobility.Mapper.Interface.Shared;
using BT.SaaS.HD.Mobility.Modules.SimReplacement.Configuration.Interfaces;
using BT.SaaS.HD.Mobility.Services;
using BT.SaaS.HD.Mobility.Services.Interfaces;
using BT.SaaS.HD.Mobility.Utility;
using BT.SaaS.HD.Mobility.ViewModels.SimReplacement;
using BT.SaaS.HD.UI.Common.Configuration.ViewModelContext;
using BT.SaaS.HD.UI.Common.ModuleHandlerFactory;
using BT.SaaS.HD.UI.Common.Response;
using BT.SaaS.HD.Utils.Extensions;
using L2CServices = BT.SaaS.HD.L2CJourney.Services.ConfigurationPage;

namespace BT.SaaS.HD.Mobility.Modules.SimReplacement.Configuration.Implementation
{
    public class SimReplacementWaivingChargeModuleHandler : ISimReplacementWaivingChargeModuleHandler
    {

        private readonly ModuleHandlerAggregator _aggregator;
        private IReasonsMapper _reasonsMapper;
        private IConfigurationPageService _mobilityConfigurationPageService;
        private ISimHelper _simHelper;
        private IBasketFacade _basketFacade;


        public SimReplacementWaivingChargeModuleHandler()
            : this(ModuleHandlerAggregator.Current)
        {


        }

        public SimReplacementWaivingChargeModuleHandler(ModuleHandlerAggregator aggregator)
        {
            _aggregator = aggregator;
            _aggregator.RegisterType<ConfigurationPageService>();
            _aggregator.RegisterType<L2CServices.ConfigurationPageService>();
            _aggregator.RegisterType<ReasonsMapper>();
            _aggregator.RegisterType<SimHelper>();
            _aggregator.RegisterType<SessionFacade>();

        }
        public WaiveChargeReasonsViewModel Initialize(ProductsViewModelContext viewModelContext)
        {

            _basketFacade = _aggregator.TryCreateOrGet<ISessionFacade>().Basket;
            _mobilityConfigurationPageService = _aggregator.TryCreateOrGet<IConfigurationPageService>();
            _reasonsMapper = _aggregator.TryCreateOrGet<IReasonsMapper>();
            _simHelper = _aggregator.TryCreateOrGet<ISimHelper>();


            var replacementChargeProduct = _simHelper.GetChargeTypeProduct(_simHelper.GetSimTypeItems().First().ProductCode);

            if (replacementChargeProduct.IsNull())
                return new WaiveChargeReasonsViewModel();

            var basketItem = _basketFacade.GetItemForProductCode(replacementChargeProduct.Code);

            return new WaiveChargeReasonsViewModel
            {
                IsChargeWaived = basketItem.IsNotNull() && IsWaivedBasketItem(basketItem),
                SelectedItem = GetSelectedWaiveChargeReason(),
                IsApplicable = true,
                ChargeTypeProductCode = replacementChargeProduct.Code,
                DefaultTariff = replacementChargeProduct.DefaultTariff.ToDecimal().FormatPrice(),
                Items = _reasonsMapper.Map(_mobilityConfigurationPageService.GetUIJourneyReasonsService(viewModelContext.ResellerId, Constants.SimWaiverChargeReasons)),
            };
        }

        internal virtual bool IsWaivedBasketItem(IBasketItemFacade basketItem)
        {
            if (basketItem.IsNull())
                return false;

            return string.Equals(basketItem.GetAttribute(Constants.IsWaive).Value, "Y", StringComparison.OrdinalIgnoreCase);
        }

        internal virtual string GetSelectedWaiveChargeReason()
        {
            _simHelper = _aggregator.TryCreateOrGet<ISimHelper>();
            var simItem = _simHelper.GetSimTypeItemWithActionAdd();
            if (simItem.IsNull())
                return Constants.None;
            var waiveReason = simItem.OrderItemInfo.FirstOrDefault(item =>
                    string.Equals(item.Name, Constants.WaiveReason, StringComparison.OrdinalIgnoreCase) &&
                    string.Equals(item.Type, Constants.ReplacementSIM, StringComparison.OrdinalIgnoreCase));
            if (waiveReason.IsNull())
                return Constants.None;
            return waiveReason.Value;
        }

        public void Update(WaiveChargeReasonsViewModel viewModel, ProductsViewModelContext context)
        {
            _simHelper = _aggregator.TryCreateOrGet<ISimHelper>();
            if (!viewModel.IsChargeWaived)
                return;

            var simItem = _simHelper.GetSimTypeItemWithActionAdd();
            simItem.AddOrderItemInfo(Constants.ReplacementSIM, Constants.ReplacementSIM, Constants.WaiveReason, viewModel.SelectedItem);
        }

        public virtual void WaiveSimReplacementCharge(string chargeTypeProductCode, bool isSimReplacementChargeWaived)
        {
            _simHelper = _aggregator.TryCreateOrGet<ISimHelper>();
            var isWaive = isSimReplacementChargeWaived ? "Y" : "N";
            _simHelper.SetIsWaiveAttribute(chargeTypeProductCode, isWaive);
        }
        
    }
}
