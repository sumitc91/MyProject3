using BT.SaaS.Core.Entity.Utils.Facades;
using BT.SaaS.Core.UI.Framework.BusinessEntities;
using BT.SaaS.HD.Mobility.Utility;
using BT.SaaS.HD.UI.Common.ModuleHandlerFactory;
using BT.SaaS.HD.UI.Common.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BT.SaaS.HD.Mobility.ContextBuilder.Shared
{
    interface INumberReservationContextBuilder
    {
        ReserveNumberOnMnumRequest BuildMNUMRequest();
    }
    public class NumberReservationContextBuilder : INumberReservationContextBuilder
    {
        public ModuleHandlerAggregator Aggregator { get; set; }
        public NumberReservationContextBuilder() : this(ModuleHandlerAggregator.Current) { }

        public NumberReservationContextBuilder(ModuleHandlerAggregator _aggregator)
        {
            Aggregator = _aggregator;
            Aggregator.RegisterType<SessionFacade>();
        }
        public virtual ReserveNumberOnMnumRequest BuildMNUMRequest()
        {
            var basket = Aggregator.TryCreateOrGet<ISessionFacade>().GetBasketFromSession();
                return new ReserveNumberOnMnumRequest
             {
                 ProductCode = Constants.BusinessMobile,
                 ContiguousFlag = Constants.ContiguousFlag,
                 CustomerKey = Aggregator.TryCreateOrGet<ISessionFacade>().CustomerData.CustomerID,
                 AreaCode = Constants.AreaCode,
                 Quantity = 1,
                 TransactionId = basket.BasketId,
                 SearchFilter = Constants.SearchFilter
             };

        }
    }
}