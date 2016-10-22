using System.Web.Mvc;
using BT.SaaS.HD.L2CJourney.ViewModels.ConfigurationPage.MaintenanceModule;
using BT.SaaS.HD.Mobility.Modules;
using BT.SaaS.HD.Mobility.Modules.Context;
using BT.SaaS.HD.Mobility.Modules.ContextBuilder.Configuration;
using BT.SaaS.HD.Mobility.ViewModels;
using BT.SaaS.HD.UI.Common.ActionFilters;
using BT.SaaS.HD.UI.Common.Configuration.ViewModelContext;
using BT.SaaS.HD.UI.Common.Configuration.ViewModels;
using BT.SaaS.HD.UI.Common.ContextBuilder;
using BT.SaaS.HD.UI.Common.ModuleHandlerFactory;
using BT.SaaS.HD.UI.Common.Modules;
using BT.SaaS.HD.UI.Common.ViewModelContext;
using BT.SaaS.HD.UI.Common.ViewModels;
using BT.SaaS.HD.Mobility.Controllers.Provide;
using BT.SaaS.HD.Mobility.Modules.Configuration;
using BT.SaaS.HD.Mobility.Modules.Configuration.BTWifi.NotNeeded;
using BT.SaaS.HD.Mobility.ViewModels.Provide.Accessory;
using BT.SaaS.HD.Mobility.Controllers.Provide.Helper;
using BT.SaaS.HD.Mobility.Controllers;
using Moq;
using NUnitFw = NUnit.Framework;

namespace BT.SaaS.HD.Mobility.Tests.Controllers
{
	[NUnitFw.TestFixture]
	public class ProvideConfigurationControllerTest
	{
		private Mock<ModuleHandlerAggregator> _moduleHandlerAggregator;
		private Mock<ProvideConfigurationController> _provideConfigurationController;
		private Mock<IModuleHandler<PoiProductsViewModel, PoiProductsViewModelContext>> _moduleHandlerPoiProductsViewModel;
		private Mock<IModuleHandler<ContractLengthViewModel, ProductsViewModelContext>> _moduleHandlerContractLengthViewModel;
		private Mock<IModuleHandler<ExtrasViewModel, ProductsViewModelContext>> _moduleHandlerExtrasViewModel;
		private Mock<IModuleHandler<SimTypeViewModel, ProductsViewModelContext>> _moduleHandlerSimTypeViewModel;
		private Mock<IBroadcastModuleHandler<ChargesViewModel, MPBSummaryViewModelContext>> _broadcastModuleHandlerChargesViewModel;
		private Mock<IModuleHandler<PricePlanViewModel, ProductsViewModelContext>> _moduleHandlerPricePlanViewModel;
		private Mock<ConfigurationContextBuilder> _configurationContextBuilder;
		private Mock<IModuleHandler<AccessoryListViewModel, ProductsViewModelContext>> _moduleHandlerAccessoryListViewModel;
		private Mock<IBroadcastModuleHandler<FooterModuleViewModel, FooterModuleContext>> _broadcastModuleHandlerFooterModuleViewModel;
		private Mock<ConfigurationModuleContextBuilder> _configurationModuleContextBuilder;

		[NUnitFw.SetUp]
		public void Setup()
		{
			_moduleHandlerAggregator = new Mock<ModuleHandlerAggregator>();
			_moduleHandlerPoiProductsViewModel = new Mock<IModuleHandler<PoiProductsViewModel, PoiProductsViewModelContext>>();
			_moduleHandlerContractLengthViewModel = new Mock<IModuleHandler<ContractLengthViewModel, ProductsViewModelContext>>();
			_moduleHandlerExtrasViewModel = new Mock<IModuleHandler<ExtrasViewModel, ProductsViewModelContext>>();
			_moduleHandlerSimTypeViewModel = new Mock<IModuleHandler<SimTypeViewModel, ProductsViewModelContext>>();
			_broadcastModuleHandlerChargesViewModel = new Mock<IBroadcastModuleHandler<ChargesViewModel, MPBSummaryViewModelContext>>();
			_moduleHandlerPricePlanViewModel = new Mock<IModuleHandler<PricePlanViewModel, ProductsViewModelContext>>();
			_configurationContextBuilder = new Mock<ConfigurationContextBuilder>();
			_moduleHandlerAccessoryListViewModel = new Mock<IModuleHandler<AccessoryListViewModel, ProductsViewModelContext>>();
			_broadcastModuleHandlerFooterModuleViewModel = new Mock<IBroadcastModuleHandler<FooterModuleViewModel, FooterModuleContext>>();
			_configurationModuleContextBuilder = new Mock<ConfigurationModuleContextBuilder>();

			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IModuleHandler<PoiProductsViewModel, PoiProductsViewModelContext>>()).Returns(_moduleHandlerPoiProductsViewModel.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IModuleHandler<ContractLengthViewModel, ProductsViewModelContext>>()).Returns(_moduleHandlerContractLengthViewModel.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IModuleHandler<ExtrasViewModel, ProductsViewModelContext>>()).Returns(_moduleHandlerExtrasViewModel.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IModuleHandler<SimTypeViewModel, ProductsViewModelContext>>()).Returns(_moduleHandlerSimTypeViewModel.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IBroadcastModuleHandler<ChargesViewModel, MPBSummaryViewModelContext>>()).Returns(_broadcastModuleHandlerChargesViewModel.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IModuleHandler<PricePlanViewModel, ProductsViewModelContext>>()).Returns(_moduleHandlerPricePlanViewModel.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ConfigurationContextBuilder>()).Returns(_configurationContextBuilder.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IModuleHandler<AccessoryListViewModel, ProductsViewModelContext>>()).Returns(_moduleHandlerAccessoryListViewModel.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IBroadcastModuleHandler<FooterModuleViewModel, FooterModuleContext>>()).Returns(_broadcastModuleHandlerFooterModuleViewModel.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ConfigurationModuleContextBuilder>()).Returns(_configurationModuleContextBuilder.Object);

			_provideConfigurationController = new Mock<ProvideConfigurationController>(_moduleHandlerAggregator.Object){ CallBase = true };
		}


		[NUnitFw.Test]
		public void Index_Test()
		{

			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _provideConfigurationController.Object.Index();
			_provideConfigurationController.VerifyAll();
			//NUnitFw.Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void Previous_Test()
		{

			var viewModel = new ProvideSIMOConfigurationViewModel();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _provideConfigurationController.Object.Previous(viewModel);
			_provideConfigurationController.VerifyAll();
			//NUnitFw.Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void Next_Test()
		{

			var viewModel = new ProvideSIMOConfigurationViewModel();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _provideConfigurationController.Object.Next(viewModel);
			_provideConfigurationController.VerifyAll();
			//NUnitFw.Assert.AreEqual("compareFrom", "compareTo");
		}
	}
}
