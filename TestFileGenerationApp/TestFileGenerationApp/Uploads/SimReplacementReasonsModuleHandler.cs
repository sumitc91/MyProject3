using System;
using System.Linq;
using BT.SaaS.Core.Shared.Entities;
using BT.SaaS.HD.Mobility.Helper;
using BT.SaaS.HD.Mobility.Mapper.Implementations.Shared;
using BT.SaaS.HD.Mobility.Mapper.Implementations.SimReplacement;
using BT.SaaS.HD.Mobility.Mapper.Interface.Shared;
using BT.SaaS.HD.Mobility.Services;
using BT.SaaS.HD.Mobility.Services.Interfaces;
using BT.SaaS.HD.Mobility.Utility;
using BT.SaaS.HD.Mobility.ViewModels;
using BT.SaaS.HD.UI.Common.Configuration.ViewModelContext;
using BT.SaaS.HD.UI.Common.ModuleHandlerFactory;
using BT.SaaS.HD.UI.Common.Modules;
using BT.SaaS.HD.Mobility.Mapper.Interface.SimReplacement;

namespace BT.SaaS.HD.Mobility.Modules.SimReplacement.Configuration.Implementation
{
    public class SimReplacementReasonsModuleHandler : ISimReplacementReasonsModuleHandler
    {

        private readonly ModuleHandlerAggregator _aggregator;
        private readonly IReplacementReasonsMapper _replacementReasonsMapper;
        private readonly IConfigurationPageService _configurationPageService;
        private ISimHelper _simHelper;

        public SimReplacementReasonsModuleHandler()
            : this(ModuleHandlerAggregator.Current)
        {

        }

        public SimReplacementReasonsModuleHandler(ModuleHandlerAggregator aggregator)
        {
            _aggregator = aggregator;
            _aggregator.RegisterType<ConfigurationPageService>();
            _aggregator.RegisterType<ReplacementReasonsMapper>();
            _aggregator.RegisterType<SimHelper>();
            _replacementReasonsMapper = _aggregator.TryCreateOrGet<IReplacementReasonsMapper>();
            _configurationPageService = _aggregator.TryCreateOrGet<IConfigurationPageService>();
            _simHelper = _aggregator.TryCreateOrGet<ISimHelper>();

        }
        public SingleSelectionViewModel Initialize(ProductsViewModelContext viewModelContext)
        {
            return new SingleSelectionViewModel
            {
                Items = _replacementReasonsMapper.Map(_configurationPageService.GetUIJourneyReasonsService(viewModelContext.ResellerId, Constants.SimReplacement)),
                SelectedItem = GetSelectedReplacementReason()
            };
        }
        
        public void Update(SingleSelectionViewModel viewModel, ProductsViewModelContext context)
        {
            var simItem = _simHelper.GetSimTypeItemWithActionAdd();
            simItem.SetDeliveryReplacementReason((ReplacementType)Enum.Parse(typeof(ReplacementType), viewModel.SelectedItem, true));
        }

        internal virtual string GetSelectedReplacementReason()
        {
            var simItem = _simHelper.GetSimTypeItemWithActionAdd();
            if (simItem == null)
                return Constants.None;
            return simItem.ReplacementType;
        }

    }
}