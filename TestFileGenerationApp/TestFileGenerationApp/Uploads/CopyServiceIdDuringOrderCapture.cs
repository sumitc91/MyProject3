using BT.SaaS.Core.Entity.Utils.Facades;
using BT.SaaS.Core.UI.Framework.ServiceEntities.DataContracts.Basket;
using BT.SaaS.HD.Mobility.Services;
using BT.SaaS.HD.Mobility.Services.Interfaces;
using BT.SaaS.HD.UI.Common.Configuration.ViewModelContext;
using BT.SaaS.HD.UI.Common.ModuleHandlerFactory;
using BT.SaaS.HD.UI.Common.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BT.SaaS.HD.Utils.Extensions;
using BT.SaaS.Core.MDMAPI.Entities.Product;
using BT.SaaS.Core.Shared.Entities;
using BT.SaaS.HD.Mobility.Helper;
using BT.SaaS.HD.Mobility.Utility;

namespace BT.SaaS.HD.Mobility.Modules.Shared
{
    interface ICopyServiceIdDuringOrderCapture{
        void CopyServiceIdForAllRequiredBasketItem(List<BasketItem> basketItems,OrderActionEnum ossAction);
    }
    public class CopyServiceIdDuringOrderCapture : ICopyServiceIdDuringOrderCapture
    {
        private readonly ModuleHandlerAggregator _aggregator;
        private IConfigurationPageService _configurationPageService;
        private INumberReservationHelper numberReservationHelper;

        public CopyServiceIdDuringOrderCapture()
            : this(ModuleHandlerAggregator.Current)
        {

        }

        public CopyServiceIdDuringOrderCapture(ModuleHandlerAggregator current)
        {
            _aggregator = current;
            _aggregator.RegisterType<SessionFacade>();
            _aggregator.RegisterType<ConfigurationPageService>();
            _aggregator.RegisterType<NumberReservationHelper>();
            numberReservationHelper = _aggregator.TryCreateOrGet<INumberReservationHelper>();
            _configurationPageService = _aggregator.TryCreateOrGet<IConfigurationPageService>();

        }
        public virtual void CopyServiceIdForAllRequiredBasketItem(List<BasketItem> basketItems, OrderActionEnum ossAction)
        {

            var resellerId = _aggregator.TryCreateOrGet<ISessionFacade>().CustomerData.ResellerID;
            var productCodeList = numberReservationHelper.GetAllUniqueScodesFromBasket(basketItems);
            GetProductDefinitonAndProcessFor(productCodeList, resellerId, basketItems, ossAction);
           
        }

        internal virtual void GetProductDefinitonAndProcessFor(List<string> productCodeList, string resellerId, List<BasketItem> basketItems,OrderActionEnum ossAction)
        {
            
            foreach (var productCode in productCodeList)
            {
                var productDefinition = _configurationPageService.GetProductDefinitionByCode(resellerId, productCode);
                if (productDefinition.ServiceIDAllocationMethod.EqualsIgnoreCase(Constants.CopyDuringOrderCapture))
                {
                    if (!productDefinition.ServiceIDControllerAttributeList.IsNullOrEmpty())
                    {
                        CopyServiceIdFrom(basketItems,productDefinition,ossAction);
                    }

                }
            }
        }

        internal virtual void CopyServiceIdFrom(List<BasketItem> basketItems, ProductDefinition productDefinition, OrderActionEnum ossAction)
        {
            var relatedItem = numberReservationHelper.GetRelatedProductsFrom(productDefinition, basketItems);
            foreach (var attr in productDefinition.ServiceIDControllerAttributeList)
            {
                if (attr.attributeName.EqualsIgnoreCase(Constants.SourceOffering))
                {
                    var sourceBasketItem = GetSourceProduct(basketItems, attr.attributeValue);
                    if (sourceBasketItem != null)
                    {
                       foreach(var item in relatedItem)
                       {
                           numberReservationHelper.UpdatePreviousServiceIdForModifyOssAction(item, ossAction);
                           item.ServiceId = sourceBasketItem.ServiceId;
                       }
                       
                    }
                }
            }
        }

        internal virtual BasketItem GetSourceProduct(List<BasketItem> basketItems, string scode)
        {
            return basketItems.Find(bItem => bItem.ProductCode.EqualsIgnoreCase(scode));
        }
    }
}