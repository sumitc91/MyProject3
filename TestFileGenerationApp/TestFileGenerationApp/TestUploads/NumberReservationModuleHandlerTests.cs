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
using BT.SaaS.HD.Mobility.Modules.Shared;
using Moq;
using NUnitFw = NUnit.Framework;

namespace BT.SaaS.HD.Mobility.Tests.Modules.Shared
{
	[NUnitFw.TestFixture]
	public class NumberReservationModuleHandlerTest
	{
		private Mock<ModuleHandlerAggregator> _moduleHandlerAggregator;
		private Mock<NumberReservationModuleHandler> _numberReservationModuleHandler;
		private Mock<INumberReservationHelper> _numberReservationHelper;
		private Mock<ISessionFacade> _sessionFacade;
		private Mock<ICopyServiceIdDuringOrderCapture> _copyServiceIdDuringOrderCapture;
		private Mock<INumberReservationContextBuilder> _numberReservationContextBuilder;
		private Mock<IAppointmentService> _appointmentService;
		private Mock<IConfigurationPageService> _configurationPageService;

		[NUnitFw.SetUp]
		public void Setup()
		{
			_moduleHandlerAggregator = new Mock<ModuleHandlerAggregator>();
			_numberReservationHelper = new Mock<INumberReservationHelper>();
			_sessionFacade = new Mock<ISessionFacade>();
			_copyServiceIdDuringOrderCapture = new Mock<ICopyServiceIdDuringOrderCapture>();
			_numberReservationContextBuilder = new Mock<INumberReservationContextBuilder>();
			_appointmentService = new Mock<IAppointmentService>();
			_configurationPageService = new Mock<IConfigurationPageService>();

			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<INumberReservationHelper>()).Returns(_numberReservationHelper.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ISessionFacade>()).Returns(_sessionFacade.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ICopyServiceIdDuringOrderCapture>()).Returns(_copyServiceIdDuringOrderCapture.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<INumberReservationContextBuilder>()).Returns(_numberReservationContextBuilder.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IAppointmentService>()).Returns(_appointmentService.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IConfigurationPageService>()).Returns(_configurationPageService.Object);

			_numberReservationModuleHandler = new Mock<NumberReservationModuleHandler>(_moduleHandlerAggregator.Object){ CallBase = true };
		}


		[NUnitFw.Test]
		public void Initialize_Test()
		{

			var viewModelContext = new ProductsViewModelContext();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _numberReservationModuleHandler.Object.Initialize(viewModelContext);
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void UpdateNumberReservationModel_Test()
		{

			var numberReservationViewModelList = new List<NumberReservationViewModel>();
			var item = new BasketItem();
			var reserveNumberResponse = new ReservedNumbersEntity();
			var ossAction = new OrderActionEnum();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			_numberReservationModuleHandler.Object.UpdateNumberReservationModel(numberReservationViewModelList,item,reserveNumberResponse,ossAction);
		}


		[NUnitFw.Test]
		public void UpdateServiceIdFrom_Test()
		{

			var numberReservationViewModelList = new List<NumberReservationViewModel>();
			var item = new BasketItem();
			var reserveNumberResponse = new ReservedNumbersEntity();
			var ossAction = new OrderActionEnum();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			_numberReservationModuleHandler.Object.UpdateServiceIdFrom(numberReservationViewModelList,item,reserveNumberResponse,ossAction);
		}


		[NUnitFw.Test]
		public void Update_Test()
		{

			var viewModel = new NumberReservationViewModel();
			var context = new ProductsViewModelContext();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			_numberReservationModuleHandler.Object.Update(viewModel,context);
		}


		[NUnitFw.Test]
		public void ReserveNumberOnMnum_Test()
		{

			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _numberReservationModuleHandler.Object.ReserveNumberOnMnum();
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void GetAllRelatedBasketItemsForCurrentContext_Test()
		{

			var viewModelContext = new ProductsViewModelContext();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _numberReservationModuleHandler.Object.GetAllRelatedBasketItemsForCurrentContext(viewModelContext);
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void GetItemsWithAllocateDuringOrderCapture_Test()
		{

			var viewModelContext = new ProductsViewModelContext();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _numberReservationModuleHandler.Object.GetItemsWithAllocateDuringOrderCapture(viewModelContext);
			//Assert.AreEqual("compareFrom", "compareTo");
		}
	}
}
