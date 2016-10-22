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
using BT.SaaS.HD.Mobility.Modules.SimReplacement.Configuration.Implementation;
using Moq;
using NUnitFw = NUnit.Framework;

namespace BT.SaaS.HD.Mobility.Tests.Modules.SimReplacement.Configuration.Implementation
{
	[NUnitFw.TestFixture]
	public class SimReplacementWaivingChargeModuleHandlerTest
	{
		private Mock<ModuleHandlerAggregator> _moduleHandlerAggregator;
		private Mock<SimReplacementWaivingChargeModuleHandler> _simReplacementWaivingChargeModuleHandler;
		private Mock<ISessionFacade> _sessionFacade;
		private Mock<IConfigurationPageService> _configurationPageService;
		private Mock<IReasonsMapper> _reasonsMapper;
		private Mock<ISimHelper> _simHelper;

		[NUnitFw.SetUp]
		public void Setup()
		{
			_moduleHandlerAggregator = new Mock<ModuleHandlerAggregator>();
			_sessionFacade = new Mock<ISessionFacade>();
			_configurationPageService = new Mock<IConfigurationPageService>();
			_reasonsMapper = new Mock<IReasonsMapper>();
			_simHelper = new Mock<ISimHelper>();

			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ISessionFacade>()).Returns(_sessionFacade.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IConfigurationPageService>()).Returns(_configurationPageService.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IReasonsMapper>()).Returns(_reasonsMapper.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ISimHelper>()).Returns(_simHelper.Object);

			_simReplacementWaivingChargeModuleHandler = new Mock<SimReplacementWaivingChargeModuleHandler>(_moduleHandlerAggregator.Object){ CallBase = true };
		}


		[NUnitFw.Test]
		public void Initialize_Test()
		{

			var viewModelContext = new ProductsViewModelContext();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _simReplacementWaivingChargeModuleHandler.Object.Initialize(viewModelContext);
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void IsWaivedBasketItem_Test()
		{

			var basketItem = new IBasketItemFacade();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _simReplacementWaivingChargeModuleHandler.Object.IsWaivedBasketItem(basketItem);
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void GetSelectedWaiveChargeReason_Test()
		{

			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _simReplacementWaivingChargeModuleHandler.Object.GetSelectedWaiveChargeReason();
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void Update_Test()
		{

			var viewModel = new WaiveChargeReasonsViewModel();
			var context = new ProductsViewModelContext();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			_simReplacementWaivingChargeModuleHandler.Object.Update(viewModel,context);
		}


		[NUnitFw.Test]
		public void WaiveSimReplacementCharge_Test()
		{

			var chargeTypeProductCode = "";
			var isSimReplacementChargeWaived = new bool();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			_simReplacementWaivingChargeModuleHandler.Object.WaiveSimReplacementCharge(chargeTypeProductCode,isSimReplacementChargeWaived);
		}
	}
}
