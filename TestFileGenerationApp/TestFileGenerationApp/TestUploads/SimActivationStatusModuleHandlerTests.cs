using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BT.SaaS.Core.Entity.Utils.Facades;
using BT.SaaS.Core.Shared.Entities;
using BT.SaaS.HD.Mobility.Helper;
using BT.SaaS.HD.Mobility.Mapper.Implementations.Shared;
using BT.SaaS.HD.Mobility.Mapper.Interface.Shared;
using BT.SaaS.HD.Mobility.Utility;
using BT.SaaS.HD.Mobility.ViewModels;
using BT.SaaS.HD.UI.Common.Configuration.ViewModelContext;
using BT.SaaS.HD.UI.Common.ModuleHandlerFactory;
using BT.SaaS.HD.UI.Common.Modules;
using BT.SaaS.HD.Mobility.Mapper.Interface.SimReplacement;
using Moq;
using NUnitFw = NUnit.Framework;

namespace BT.SaaS.HD.Mobility.Tests.Modules.SimReplacement.Configuration.Implementation
{
	[NUnitFw.TestFixture]
	public class SimActivationStatusModuleHandlerTest
	{
		private Mock<ModuleHandlerAggregator> _moduleHandlerAggregator;
		private SimActivationStatusModuleHandler _simActivationStatusModuleHandler;
		private Mock<IBasketUtils> _basketUtils;
		private Mock<ISelectListItemMapper> _selectListItemMapper;
		private Mock<ISimHelper> _simHelper;

		[NUnitFw.SetUp]
		public void Setup()
		{
			_moduleHandlerAggregator = new Mock<ModuleHandlerAggregator>();
			_basketUtils = new Mock<IBasketUtils>();
			_selectListItemMapper = new Mock<ISelectListItemMapper>();
			_simHelper = new Mock<ISimHelper>();

			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IBasketUtils>()).Returns(_basketUtils.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ISelectListItemMapper>()).Returns(_selectListItemMapper.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ISimHelper>()).Returns(_simHelper.Object);

			_simActivationStatusModuleHandler = new SimActivationStatusModuleHandler(_moduleHandlerAggregator.Object);
		}


		[NUnitFw.Test]
		public void Initialize_Test()
		{

			var viewModelContext = new ProductsViewModelContext();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _simActivationStatusModuleHandler.Initialize(viewModelContext);
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void GetSelectedSimActivationStatus_Test()
		{

			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _simActivationStatusModuleHandler.GetSelectedSimActivationStatus();
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void GetAllowedSimActivationOptionsFromBasket_Test()
		{

			var items = new IBasketItemsFacade();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _simActivationStatusModuleHandler.GetAllowedSimActivationOptionsFromBasket(items);
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void Update_Test()
		{

			var viewModel = new SingleSelectionViewModel();
			var context = new ProductsViewModelContext();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			_simActivationStatusModuleHandler.Update(viewModel,context);
		}
	}
}
