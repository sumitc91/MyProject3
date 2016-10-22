using System.Linq;
using BT.SaaS.Core.Entity.Utils.Facades;
using BT.SaaS.Core.Shared.Entities;
using BT.SaaS.Core.UI.Framework.ServiceEntities;
using BT.SaaS.HD.Mobility.ContextBuilder.SimReplacement;
using BT.SaaS.HD.Mobility.Helper;
using BT.SaaS.HD.Mobility.Helper.Interfaces;
using BT.SaaS.HD.Mobility.Modules.ContextBuilder.Configuration;
using BT.SaaS.HD.Mobility.Modules.Modify;
using BT.SaaS.HD.Mobility.Modules.SimReplacement.Configuration.Interfaces;
using BT.SaaS.HD.Mobility.Modules.SimReplacement.Processor;
using BT.SaaS.HD.Mobility.Utility;
using BT.SaaS.HD.Mobility.ViewModels;
using BT.SaaS.HD.Mobility.ViewModels.SimReplacement;
using BT.SaaS.HD.UI.Common.ModuleHandlerFactory;
using BT.SaaS.HD.UI.Common.Modules;
using BT.SaaS.HD.UI.Common.Modules.Configuration;
using BT.SaaS.HD.UI.Common.ViewModelContext;
using BT.SaaS.HD.UI.Common.ViewModels;
using BT.SaaS.HD.Mobility.Mapper.Interface.SimReplacement;
using BT.SaaS.HD.UI.Common.Configuration.ViewModelContext;
using BT.SaaS.HD.Mobility.Modules.SimReplacement.Configuration.Implementation;
using Moq;
using NUnitFw = NUnit.Framework;

namespace BT.SaaS.HD.Mobility.Tests.Modules.SimReplacement.Configuration.Implementation
{
	[NUnitFw.TestFixture]
	public class SimReplacementInvokerTest
	{
		private Mock<ModuleHandlerAggregator> _moduleHandlerAggregator;
		private Mock<SimReplacementInvoker> _simReplacementInvoker;
		private Mock<ISimModuleHandler> _simModuleHandler;
		private Mock<ConfigurationContextBuilder> _configurationContextBuilder;
		private Mock<IResetBasketHandler> _resetBasketHandler;
		private Mock<ISimReplacementContextBuilder> _simReplacementContextBuilder;
		private Mock<ISimActivationStatusModuleHandler> _simActivationStatusModuleHandler;
		private Mock<ISimReplacementReasonsModuleHandler> _simReplacementReasonsModuleHandler;
		private Mock<ISimReplacementWaivingChargeModuleHandler> _simReplacementWaivingChargeModuleHandler;
		private Mock<IBroadcastModuleHandler<FooterModuleViewModel, FooterModuleContext>> _broadcastModuleHandlerFooterModuleViewModel;
		private Mock<ISessionFacade> _sessionFacade;
		private Mock<T2RAssetProcessor> _t2RAssetProcessor;
		private Mock<L2CAssetProcessor> _l2CAssetProcessor;
		private Mock<IModuleHandler<SimTypeViewModel, ProductsViewModelContext>> _moduleHandlerSimTypeViewModel;
		private Mock<IPortalNavigationHelper> _portalNavigationHelper;

		[NUnitFw.SetUp]
		public void Setup()
		{
			_moduleHandlerAggregator = new Mock<ModuleHandlerAggregator>();
			_simModuleHandler = new Mock<ISimModuleHandler>();
			_configurationContextBuilder = new Mock<ConfigurationContextBuilder>();
			_resetBasketHandler = new Mock<IResetBasketHandler>();
			_simReplacementContextBuilder = new Mock<ISimReplacementContextBuilder>();
			_simActivationStatusModuleHandler = new Mock<ISimActivationStatusModuleHandler>();
			_simReplacementReasonsModuleHandler = new Mock<ISimReplacementReasonsModuleHandler>();
			_simReplacementWaivingChargeModuleHandler = new Mock<ISimReplacementWaivingChargeModuleHandler>();
			_broadcastModuleHandlerFooterModuleViewModel = new Mock<IBroadcastModuleHandler<FooterModuleViewModel, FooterModuleContext>>();
			_sessionFacade = new Mock<ISessionFacade>();
			_t2RAssetProcessor = new Mock<T2RAssetProcessor>();
			_l2CAssetProcessor = new Mock<L2CAssetProcessor>();
			_moduleHandlerSimTypeViewModel = new Mock<IModuleHandler<SimTypeViewModel, ProductsViewModelContext>>();
			_portalNavigationHelper = new Mock<IPortalNavigationHelper>();

			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ISimModuleHandler>()).Returns(_simModuleHandler.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ConfigurationContextBuilder>()).Returns(_configurationContextBuilder.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IResetBasketHandler>()).Returns(_resetBasketHandler.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ISimReplacementContextBuilder>()).Returns(_simReplacementContextBuilder.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ISimActivationStatusModuleHandler>()).Returns(_simActivationStatusModuleHandler.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ISimReplacementReasonsModuleHandler>()).Returns(_simReplacementReasonsModuleHandler.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ISimReplacementWaivingChargeModuleHandler>()).Returns(_simReplacementWaivingChargeModuleHandler.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IBroadcastModuleHandler<FooterModuleViewModel, FooterModuleContext>>()).Returns(_broadcastModuleHandlerFooterModuleViewModel.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ISessionFacade>()).Returns(_sessionFacade.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<T2RAssetProcessor>()).Returns(_t2RAssetProcessor.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<L2CAssetProcessor>()).Returns(_l2CAssetProcessor.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IModuleHandler<SimTypeViewModel, ProductsViewModelContext>>()).Returns(_moduleHandlerSimTypeViewModel.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IPortalNavigationHelper>()).Returns(_portalNavigationHelper.Object);

			_simReplacementInvoker = new Mock<SimReplacementInvoker>(_moduleHandlerAggregator.Object){ CallBase = true };
		}


		[NUnitFw.Test]
		public void AddSimTypeProduct_Test()
		{

			var productCode = "";
			var parentProductCode = "";
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _simReplacementInvoker.Object.AddSimTypeProduct(productCode,parentProductCode);
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void GetSimReplacementViewModel_Test()
		{

			var isReadOnly = new bool();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _simReplacementInvoker.Object.GetSimReplacementViewModel(isReadOnly);
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void ProcessAssetBasedOnSource_Test()
		{

			var source = "";
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			_simReplacementInvoker.Object.ProcessAssetBasedOnSource(source);
		}


		[NUnitFw.Test]
		public void GetSource_Test()
		{

			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _simReplacementInvoker.Object.GetSource();
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void SetBasketScenario_Test()
		{

			var source = "";
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			_simReplacementInvoker.Object.SetBasketScenario(source);
		}


		[NUnitFw.Test]
		public void GetInstanceOfSource_Test()
		{

			var source = "";
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _simReplacementInvoker.Object.GetInstanceOfSource(source);
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void UpdateWaiveAttributeOfSimReplacementChargeProduct_Test()
		{

			var chargeTypeProductCode = "";
			var isSimReplacementChargedWaived = new bool();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _simReplacementInvoker.Object.UpdateWaiveAttributeOfSimReplacementChargeProduct(chargeTypeProductCode,isSimReplacementChargedWaived);
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void Update_Test()
		{

			var simReplacementViewModel = new SimReplacementViewModel();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			_simReplacementInvoker.Object.Update(simReplacementViewModel);
		}


		[NUnitFw.Test]
		public void Navigate_Test()
		{

			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _simReplacementInvoker.Object.Navigate();
			//Assert.AreEqual("compareFrom", "compareTo");
		}
	}
}
