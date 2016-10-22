using BT.SaaS.Core.MDMAPI.Entities.Product;
using BT.SaaS.Core.UI.Framework.ServiceEntities.DataContracts.Basket;
using BT.SaaS.HD.UI.Common.Configuration.ViewModelContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BT.SaaS.HD.Utils.Extensions;
using BT.SaaS.Core.Shared.Entities;
using Moq;
using NUnitFw = NUnit.Framework;

namespace BT.SaaS.HD.Mobility.Tests.Helper
{
	[NUnitFw.TestFixture]
	public class NumberReservationHelperTest
	{
		private Mock<ModuleHandlerAggregator> _moduleHandlerAggregator;
		private NumberReservationHelper _numberReservationHelper;

		[NUnitFw.SetUp]
		public void Setup()
		{
			_moduleHandlerAggregator = new Mock<ModuleHandlerAggregator>();


			_numberReservationHelper = new NumberReservationHelper(_moduleHandlerAggregator.Object);
		}


		[NUnitFw.Test]
		public void GetAllUniqueScodesFromBasket_Test()
		{

			var basketItems = new List<BasketItem>();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _numberReservationHelper.GetAllUniqueScodesFromBasket(basketItems);
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void GetRelatedProductsFrom_Test()
		{

			var productDefinition = new ProductDefinition();
			var basketItems = new List<BasketItem>();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _numberReservationHelper.GetRelatedProductsFrom(productDefinition,basketItems);
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void GetBasketItemWithReservationKey_Test()
		{

			var viewModelContext = new ProductsViewModelContext();
			var basket = new Basket();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _numberReservationHelper.GetBasketItemWithReservationKey(viewModelContext,basket);
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void UpdatePreviousServiceIdForModifyOssAction_Test()
		{

			var item = new BasketItem();
			var ossAction = new OrderActionEnum();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			_numberReservationHelper.UpdatePreviousServiceIdForModifyOssAction(item,ossAction);
		}
	}
}
