using BT.SaaS.Core.MDMAPI.Entities.Product;
using BT.SaaS.Core.UI.Framework.ServiceEntities.DataContracts.Basket;
using BT.SaaS.HD.UI.Common.Configuration.ViewModelContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BT.SaaS.HD.Utils.Extensions;
using BT.SaaS.Core.Shared.Entities;

namespace BT.SaaS.HD.Mobility.Helper
{
    interface INumberReservationHelper
    {
        List<string> GetAllUniqueScodesFromBasket(List<BasketItem> basketItems);

        List<BasketItem> GetRelatedProductsFrom(ProductDefinition productDefinition, List<BasketItem> basketItems);

        BasketItem GetBasketItemWithReservationKey(ProductsViewModelContext viewModelContext, Basket basket);

        void UpdatePreviousServiceIdForModifyOssAction(BasketItem item, OrderActionEnum ossAction);
    }
    public class NumberReservationHelper : INumberReservationHelper
    {
        public virtual List<string> GetAllUniqueScodesFromBasket(List<BasketItem> basketItems)
        {
            var listOfProdCode = basketItems
                .FindAllElements(pi => pi != null && !string.IsNullOrEmpty(pi.ProductCode)
                , bItem => bItem.AddOns).Select(p => p.ProductCode).Distinct().ToList();
            return listOfProdCode;
        }
        public virtual List<BasketItem> GetRelatedProductsFrom(ProductDefinition productDefinition, List<BasketItem> basketItems)
        {

            var flattenedBasketItems = basketItems
                  .FindAllElements(pi => pi != null && !string.IsNullOrEmpty(pi.ProductCode)
                  , bItem => bItem.AddOns).ToList();

            return flattenedBasketItems.FindAll(fItem => fItem.ProductCode.EqualsIgnoreCase(productDefinition.Code));


        }
        public virtual BasketItem GetBasketItemWithReservationKey(ProductsViewModelContext viewModelContext,Basket basket)
        {

            var relatedItems= basket.BasketItems.FindAll(bItem => bItem.LinkedLeadProductId.EqualsIgnoreCase(viewModelContext.Products.BasketItems.First().LinkedLeadProductId));
            if (relatedItems.IsNullOrEmpty()) { return null; }
            return relatedItems.Find(bItem => !bItem.NumberReservationKey.IsNullOrEmpty());
        }
        public virtual void UpdatePreviousServiceIdForModifyOssAction(BasketItem item, OrderActionEnum ossAction)
        {
            if (ossAction == OrderActionEnum.modifyAsset)
            {
                item.PreviousServiceId = item.PreviousServiceId.IsNullOrEmpty()
                    ? item.ServiceId
                    : item.PreviousServiceId;
            }
        }
      

    }
}