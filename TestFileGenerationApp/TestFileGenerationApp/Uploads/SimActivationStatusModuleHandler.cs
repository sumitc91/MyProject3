using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BT.SaaS.Core.Entity.Utils.Facades;
using BT.SaaS.Core.Shared.Entities;
using BT.SaaS.HD.Mobility.Helper;
using BT.SaaS.HD.Mobility.Mapper.Implementations.Shared;
using BT.SaaS.HD.Mobility.Mapper.Interface.Shared;
using BT.SaaS.HD.Mobility.Utility;
using BT.SaaS.HD.Mobility.ViewModels;
using BT.SaaS.HD.UI.Common.Configuration.ViewModelContext;
using BT.SaaS.HD.UI.Common.ModuleHandlerFactory;
using BT.SaaS.HD.UI.Common.Modules;
using BT.SaaS.HD.Mobility.Mapper.Interface.SimReplacement;

namespace BT.SaaS.HD.Mobility.Modules.SimReplacement.Configuration.Implementation
{
    public class SimActivationStatusModuleHandler : ISimActivationStatusModuleHandler
    {

        private readonly ModuleHandlerAggregator _aggregator;
        private IBasketUtils _basketUtils;
        private ISelectListItemMapper _selectListItemMapper;
        private ISimHelper _simHelper;

        public SimActivationStatusModuleHandler()
            : this(ModuleHandlerAggregator.Current)
        {

        }

        public SimActivationStatusModuleHandler(ModuleHandlerAggregator aggregator)
        {
            _aggregator = aggregator;
            _aggregator.RegisterType<BasketUtils>();
            _aggregator.RegisterType<SessionFacade>();
            _aggregator.RegisterType<SelectListItemMapper>();
            _aggregator.RegisterType<SimHelper>();
            _basketUtils = _aggregator.TryCreateOrGet<IBasketUtils>();
            _selectListItemMapper = _aggregator.TryCreateOrGet<ISelectListItemMapper>();
            _simHelper = _aggregator.TryCreateOrGet<ISimHelper>();
        }
        public SingleSelectionViewModel Initialize(ProductsViewModelContext viewModelContext)
        {
            var relatedBasketItems = _aggregator.TryCreateOrGet<IBasketUtils>().GetCurrentRelatedItemInPromotionWithGroupName(viewModelContext.Products, new List<string>() { Constants.MobileAccessGroupName });
            return new SingleSelectionViewModel
            {
                Items = GetAllowedSimActivationOptionsFromBasket(relatedBasketItems),
                SelectedItem = GetSelectedSimActivationStatus()
            };
        }

        internal virtual string GetSelectedSimActivationStatus()
        {
            var simTypeBasketItem = _simHelper.GetSimTypeItemWithActionAdd();
            if (simTypeBasketItem == null)
                 return Constants.NotActivatedSimStatus;

            return simTypeBasketItem.GetAttribute(Constants.ActivatedStatusAttributeName).Value;
        }

        internal virtual List<SelectListItem> GetAllowedSimActivationOptionsFromBasket(IBasketItemsFacade items)
        {
            var simTypeItem = _simHelper.GetAllSimProductsHavingReplaceWithDependency().First();          

            var simActivationStatusAllowedValues = _simHelper.GetAllowedValuesForProductByAttributeName(simTypeItem.Code, Constants.ActivatedStatusAttributeName);

            return simActivationStatusAllowedValues.Select(allowedValue => _selectListItemMapper.Map(allowedValue.Key, allowedValue.Value)).ToList();
        }
        
        public void Update(SingleSelectionViewModel viewModel, ProductsViewModelContext context)
        {
            var simItem = _simHelper.GetSimTypeItemWithActionAdd();
            simItem.SetAttributeValue(Constants.ActivatedStatusAttributeName, viewModel.SelectedItem);
        }
    }
}