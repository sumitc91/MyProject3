using BT.SaaS.HD.Mobility.Modules;
using BT.SaaS.HD.Mobility.Modules.SimReplacement.Configuration.Implementation;
using BT.SaaS.HD.Mobility.Modules.SimReplacement.Configuration.Interfaces;
using BT.SaaS.HD.Mobility.ViewModels.SimReplacement;
using System.Web.Mvc;
using UI.Common.ActionFilters;
using UI.Common.ModuleHandlerFactory;
using UI.Common.Controllers;
using BT.SaaS.HD.Mobility.Modules.ContextBuilder.Configuration;
using BT.SaaS.HD.Mobility.Utility;
using BT.SaaS.HD.Mobility.Controllers.SimReplacement;
using Moq;
using NUnitFw = NUnit.Framework;

namespace BT.SaaS.HD.Mobility.Tests.Controllers.SimReplacement
{
	[NUnitFw.TestFixture]
	public class SimReplacementControllerTest
	{
		private Mock<ModuleHandlerAggregator> _moduleHandlerAggregator;
		private Mock<SimReplacementController> _simReplacementController;
		private Mock<ISimReplacementInvoker> _simReplacementInvoker;
		private Mock<ISimModuleHandler> _simModuleHandler;
		private Mock<ISimReplacementWaivingChargeModuleHandler> _simReplacementWaivingChargeModuleHandler;
		private Mock<ConfigurationContextBuilder> _configurationContextBuilder;
		private Mock<Modules.Modify.IResetBasketHandler> _modules.Modify.IResetBasketHandler;
		private Mock<Modules.Modify.ISimTypeModuleHandler> _modules.Modify.ISimTypeModuleHandler;

		[NUnitFw.SetUp]
		public void Setup()
		{
			_moduleHandlerAggregator = new Mock<ModuleHandlerAggregator>();
			_simReplacementInvoker = new Mock<ISimReplacementInvoker>();
			_simModuleHandler = new Mock<ISimModuleHandler>();
			_simReplacementWaivingChargeModuleHandler = new Mock<ISimReplacementWaivingChargeModuleHandler>();
			_configurationContextBuilder = new Mock<ConfigurationContextBuilder>();
			_modules_Modify_IResetBasketHandler = new Mock<Modules.Modify.IResetBasketHandler>();
			_modules_Modify_ISimTypeModuleHandler = new Mock<Modules.Modify.ISimTypeModuleHandler>();

			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ISimReplacementInvoker>()).Returns(_simReplacementInvoker.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ISimModuleHandler>()).Returns(_simModuleHandler.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ISimReplacementWaivingChargeModuleHandler>()).Returns(_simReplacementWaivingChargeModuleHandler.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ConfigurationContextBuilder>()).Returns(_configurationContextBuilder.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<Modules.Modify.IResetBasketHandler>()).Returns(_modules_Modify_IResetBasketHandler.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<Modules.Modify.ISimTypeModuleHandler>()).Returns(_modules_Modify_ISimTypeModuleHandler.Object);

			_simReplacementController = new Mock<SimReplacementController>(_moduleHandlerAggregator.Object){ CallBase = true };
		}


		[NUnitFw.Test]
		public void Index_Test()
		{

			var isReadOnly = new bool();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _simReplacementController.Object.Index(isReadOnly);
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void AddProductForSimType_Test()
		{

			var productCode = "";
			var parentProductCode = "";
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _simReplacementController.Object.AddProductForSimType(productCode,parentProductCode);
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void WaiveSimReplacementCharge_Test()
		{

			var chargeTypeProductCode = "";
			var isSimReplacementChargedWaived = new bool();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _simReplacementController.Object.WaiveSimReplacementCharge(chargeTypeProductCode,isSimReplacementChargedWaived);
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void Update_Test()
		{

			var simReplacementViewModel = new SimReplacementViewModel();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _simReplacementController.Object.Update(simReplacementViewModel);
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void ReloadSimReplacement_Test()
		{

			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _simReplacementController.Object.ReloadSimReplacement();
			//Assert.AreEqual("compareFrom", "compareTo");
		}
	}
}
