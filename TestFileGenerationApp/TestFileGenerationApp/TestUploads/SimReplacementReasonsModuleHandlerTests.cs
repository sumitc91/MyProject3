using System;
using System.Linq;
using BT.SaaS.Core.Shared.Entities;
using BT.SaaS.HD.Mobility.Helper;
using BT.SaaS.HD.Mobility.Mapper.Implementations.Shared;
using BT.SaaS.HD.Mobility.Mapper.Implementations.SimReplacement;
using BT.SaaS.HD.Mobility.Mapper.Interface.Shared;
using BT.SaaS.HD.Mobility.Services;
using BT.SaaS.HD.Mobility.Services.Interfaces;
using BT.SaaS.HD.Mobility.Utility;
using BT.SaaS.HD.Mobility.ViewModels;
using BT.SaaS.HD.UI.Common.Configuration.ViewModelContext;
using BT.SaaS.HD.UI.Common.ModuleHandlerFactory;
using BT.SaaS.HD.UI.Common.Modules;
using BT.SaaS.HD.Mobility.Mapper.Interface.SimReplacement;
using BT.SaaS.HD.Mobility.Modules.SimReplacement.Configuration.Implementation;
using Moq;
using NUnitFw = NUnit.Framework;

namespace BT.SaaS.HD.Mobility.Tests.Modules.SimReplacement.Configuration.Implementation
{
	[NUnitFw.TestFixture]
	public class SimReplacementReasonsModuleHandlerTest
	{
		private Mock<ModuleHandlerAggregator> _moduleHandlerAggregator;
		private Mock<SimReplacementReasonsModuleHandler> _simReplacementReasonsModuleHandler;
		private Mock<IReplacementReasonsMapper> _replacementReasonsMapper;
		private Mock<IConfigurationPageService> _configurationPageService;
		private Mock<ISimHelper> _simHelper;

		[NUnitFw.SetUp]
		public void Setup()
		{
			_moduleHandlerAggregator = new Mock<ModuleHandlerAggregator>();
			_replacementReasonsMapper = new Mock<IReplacementReasonsMapper>();
			_configurationPageService = new Mock<IConfigurationPageService>();
			_simHelper = new Mock<ISimHelper>();

			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IReplacementReasonsMapper>()).Returns(_replacementReasonsMapper.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IConfigurationPageService>()).Returns(_configurationPageService.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ISimHelper>()).Returns(_simHelper.Object);

			_simReplacementReasonsModuleHandler = new Mock<SimReplacementReasonsModuleHandler>(_moduleHandlerAggregator.Object){ CallBase = true };
		}


		[NUnitFw.Test]
		public void Initialize_Test()
		{

			var viewModelContext = new ProductsViewModelContext();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _simReplacementReasonsModuleHandler.Object.Initialize(viewModelContext);
			_simReplacementReasonsModuleHandler.VerifyAll();
			//NUnitFw.Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void Update_Test()
		{

			var viewModel = new SingleSelectionViewModel();
			var context = new ProductsViewModelContext();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			_simReplacementReasonsModuleHandler.Object.Update(viewModel,context);
			_simReplacementReasonsModuleHandler.VerifyAll();
		}


		[NUnitFw.Test]
		public void GetSelectedReplacementReason_Test()
		{

			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _simReplacementReasonsModuleHandler.Object.GetSelectedReplacementReason();
			_simReplacementReasonsModuleHandler.VerifyAll();
			//NUnitFw.Assert.AreEqual("compareFrom", "compareTo");
		}
	}
}
