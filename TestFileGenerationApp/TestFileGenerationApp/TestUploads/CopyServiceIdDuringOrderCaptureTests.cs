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
using BT.SaaS.HD.Mobility.Modules.Shared;
using Moq;
using NUnitFw = NUnit.Framework;

namespace BT.SaaS.HD.Mobility.Tests.Modules.Shared
{
	[NUnitFw.TestFixture]
	public class CopyServiceIdDuringOrderCaptureTest
	{
		private Mock<ModuleHandlerAggregator> _moduleHandlerAggregator;
		private Mock<CopyServiceIdDuringOrderCapture> _copyServiceIdDuringOrderCapture;
		private Mock<INumberReservationHelper> _numberReservationHelper;
		private Mock<IConfigurationPageService> _configurationPageService;
		private Mock<ISessionFacade> _sessionFacade;

		[NUnitFw.SetUp]
		public void Setup()
		{
			_moduleHandlerAggregator = new Mock<ModuleHandlerAggregator>();
			_numberReservationHelper = new Mock<INumberReservationHelper>();
			_configurationPageService = new Mock<IConfigurationPageService>();
			_sessionFacade = new Mock<ISessionFacade>();

			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<INumberReservationHelper>()).Returns(_numberReservationHelper.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IConfigurationPageService>()).Returns(_configurationPageService.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ISessionFacade>()).Returns(_sessionFacade.Object);

			_copyServiceIdDuringOrderCapture = new Mock<CopyServiceIdDuringOrderCapture>(_moduleHandlerAggregator.Object){ CallBase = true };
		}


		[NUnitFw.Test]
		public void CopyServiceIdForAllRequiredBasketItem_Test()
		{

			var basketItems = new List<BasketItem>();
			var ossAction = new OrderActionEnum();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			_copyServiceIdDuringOrderCapture.Object.CopyServiceIdForAllRequiredBasketItem(basketItems,ossAction);
		}


		[NUnitFw.Test]
		public void GetProductDefinitonAndProcessFor_Test()
		{

			var productCodeList = new List<string>();
			var resellerId = "";
			var basketItems = new List<BasketItem>();
			var ossAction = new OrderActionEnum();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			_copyServiceIdDuringOrderCapture.Object.GetProductDefinitonAndProcessFor(productCodeList,resellerId,basketItems,ossAction);
		}


		[NUnitFw.Test]
		public void CopyServiceIdFrom_Test()
		{

			var basketItems = new List<BasketItem>();
			var productDefinition = new ProductDefinition();
			var ossAction = new OrderActionEnum();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			_copyServiceIdDuringOrderCapture.Object.CopyServiceIdFrom(basketItems,productDefinition,ossAction);
		}


		[NUnitFw.Test]
		public void GetSourceProduct_Test()
		{

			var basketItems = new List<BasketItem>();
			var scode = "";
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _copyServiceIdDuringOrderCapture.Object.GetSourceProduct(basketItems,scode);
			//Assert.AreEqual("compareFrom", "compareTo");
		}
	}
}
