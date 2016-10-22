using System;
using System.Collections.Generic;
using System.Linq;
using BT.SaaS.Core.Entity.Utils.Extensions;
using BT.SaaS.Core.Entity.Utils.Facades;
using BT.SaaS.Core.Shared.Entities;
using BT.SaaS.Core.UI.Framework.BusinessEntities;
using BT.SaaS.HD.Mobility.Utility;
using BT.SaaS.HD.Mobility.ViewModels;
using BT.SaaS.HD.UI.Common.Configuration.ViewModelContext;
using BT.SaaS.HD.UI.Common.ModuleHandlerFactory;
using BT.SaaS.HD.UI.Common.Modules;
using BT.SaaS.HD.UI.Common.Request;
using BT.SaaS.HD.UI.Common.Services;
using BT.SaaS.HD.Mobility.Services.Interfaces;
using BT.SaaS.HD.Mobility.Services;
using BT.SaaS.HD.Utils.Extensions;
using BT.SaaS.Core.MDMAPI.Entities.Product;
using BT.SaaS.HD.Mobility.ContextBuilder.Shared;
using BT.SaaS.Core.UI.Framework.ServiceEntities.DataContracts.Basket;
using BT.SaaS.HD.Mobility.Helper;

namespace BT.SaaS.HD.Mobility.Modules.Shared
{

    public interface INumberReservationModuleHandler : IModuleHandler<NumberReservationViewModel, ProductsViewModelContext>
    {
        NumberReservationViewModel Initialize(ProductsViewModelContext viewModelContext);
        List<BasketItem> GetItemsWithAllocateDuringOrderCapture(ProductsViewModelContext viewModelContext);
    }

    public class NumberReservationModuleHandler : INumberReservationModuleHandler
    {
        private readonly ModuleHandlerAggregator _aggregator;
        private IConfigurationPageService _configurationPageService;
        private INumberReservationHelper numberReservationHelper;

        public NumberReservationModuleHandler()
            : this(ModuleHandlerAggregator.Current)
        {

        }

        public NumberReservationModuleHandler(ModuleHandlerAggregator current)
        {
            _aggregator = current;
            _aggregator.RegisterType<AppointmentService>();
            _aggregator.RegisterType<BasketUtils>();
            _aggregator.RegisterType<SessionFacade>();
            _aggregator.RegisterType<ConfigurationPageService>();
            _aggregator.RegisterType<NumberReservationContextBuilder>();
            _aggregator.RegisterType<CopyServiceIdDuringOrderCapture>();
            _aggregator.RegisterType<NumberReservationHelper>();
            numberReservationHelper = _aggregator.TryCreateOrGet<INumberReservationHelper>();

        }

        public virtual NumberReservationViewModel Initialize(ProductsViewModelContext viewModelContext)
        {
            var numberReservationViewModelList = new List<NumberReservationViewModel>();
            var basket = _aggregator.TryCreateOrGet<ISessionFacade>().GetBasketFromSession();
            if (numberReservationHelper.GetBasketItemWithReservationKey(viewModelContext, basket) != null)
            {
                return new NumberReservationViewModel()
                {
                    ReservedNumber = viewModelContext.Products.BasketItems.First().ServiceId
                };


            }
            var itemsWithAllocatedDuringCapture = GetItemsWithAllocateDuringOrderCapture(viewModelContext);
            if (itemsWithAllocatedDuringCapture == null) { return null; }

            foreach (var item in itemsWithAllocatedDuringCapture)
            {
                var reserveNumberResponse = ReserveNumberOnMnum();

                UpdateNumberReservationModel(numberReservationViewModelList, item, reserveNumberResponse, basket.Action);

            }
            _aggregator.TryCreateOrGet<ICopyServiceIdDuringOrderCapture>().CopyServiceIdForAllRequiredBasketItem(GetAllRelatedBasketItemsForCurrentContext(viewModelContext), basket.Action);
            return numberReservationViewModelList.FirstOrDefault();


        }

        internal virtual void UpdateNumberReservationModel(List<NumberReservationViewModel> numberReservationViewModelList, BasketItem item, ReservedNumbersEntity reserveNumberResponse,OrderActionEnum ossAction)
        {
            if (reserveNumberResponse.IsNotNull() && !string.IsNullOrEmpty(reserveNumberResponse.reservationKey))
            {
                UpdateServiceIdFrom(numberReservationViewModelList, item, reserveNumberResponse, ossAction);
            }
            else
            {
                var numberReservationViewModel = new NumberReservationViewModel()
                {
                    ReservedNumber = string.Empty,
                    ReservationKey = string.Empty
                };
                numberReservationViewModelList.Add(numberReservationViewModel);
            }
        }

        internal virtual void UpdateServiceIdFrom(List<NumberReservationViewModel> numberReservationViewModelList, BasketItem item, ReservedNumbersEntity reserveNumberResponse,OrderActionEnum ossAction)
        {
            item.NumberReservationKey = reserveNumberResponse.reservationKey;
            numberReservationHelper.UpdatePreviousServiceIdForModifyOssAction(item, ossAction);
            item.ServiceId = reserveNumberResponse.numberList.First().number;

            var numberReservationViewModel = new NumberReservationViewModel()
            {
                ReservedNumber = reserveNumberResponse.numberList.First().number,
                ReservationKey = reserveNumberResponse.reservationKey
            };
            numberReservationViewModelList.Add(numberReservationViewModel);
        }


        public void Update(NumberReservationViewModel viewModel, ProductsViewModelContext context)
        {
            throw new NotImplementedException();
        }

        internal virtual ReservedNumbersEntity ReserveNumberOnMnum()
        {
            var reserveMnumRequest = _aggregator.TryCreateOrGet<INumberReservationContextBuilder>().BuildMNUMRequest();

            return _aggregator.TryCreateOrGet<IAppointmentService>().ReserveNumberOnMnum(reserveMnumRequest);

        }
        internal virtual List<BasketItem> GetAllRelatedBasketItemsForCurrentContext(ProductsViewModelContext viewModelContext)
        {
            var basket = _aggregator.TryCreateOrGet<ISessionFacade>().GetBasketFromSession();
            return basket.BasketItems.FindAll(bItem => bItem.LinkedLeadProductId.EqualsIgnoreCase(viewModelContext.Products.BasketItems.First().LinkedLeadProductId));
        }
        public virtual List<BasketItem> GetItemsWithAllocateDuringOrderCapture(ProductsViewModelContext viewModelContext)
        {
            _configurationPageService = _aggregator.TryCreateOrGet<IConfigurationPageService>();
            var resellerId = _aggregator.TryCreateOrGet<ISessionFacade>().CustomerData.ResellerID;
            var relatedBasketItems = GetAllRelatedBasketItemsForCurrentContext(viewModelContext);
            var productCodeList = numberReservationHelper.GetAllUniqueScodesFromBasket(relatedBasketItems);
            return (from productCode in productCodeList
                select _configurationPageService.GetProductDefinitionByCode(resellerId, productCode)
                into productDefinition
                where productDefinition.ServiceIDAllocationMethod.EqualsIgnoreCase(Constants.AllocatedDuringOrderCapture)
                from rItem in numberReservationHelper.GetRelatedProductsFrom(productDefinition, relatedBasketItems)
                select rItem).ToList();
        }
        
    }
}