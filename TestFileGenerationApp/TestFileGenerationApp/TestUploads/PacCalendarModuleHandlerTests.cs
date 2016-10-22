using BT.SaaS.Core.Entity.Utils.Facades;
using BT.SaaS.Core.MDMAPI.Entities.Product;
using BT.SaaS.Core.UI.Framework.ServiceEntities.ServiceContracts;
using BT.SaaS.Core.UI.Framework.Services;
using BT.SaaS.HD.L2CJourney.Services.ConfigurationPage;
using BT.SaaS.HD.Mobility.Context;
using BT.SaaS.HD.Mobility.Utility;
using BT.SaaS.HD.Mobility.ViewModels.PAC;
using BT.SaaS.HD.UI.Common.ModuleHandlerFactory;
using BT.SaaS.HD.UI.Common.Modules;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
//Adding 29 days from current days exclusing Weekends.
using BT.SaaS.HD.Mobility.Modules.PAC;
using Moq;
using NUnitFw = NUnit.Framework;

namespace BT.SaaS.HD.Mobility.Tests.Modules.PAC
{
	[NUnitFw.TestFixture]
	public class PacCalendarModuleHandlerTest
	{
		private Mock<ModuleHandlerAggregator> _moduleHandlerAggregator;
		private Mock<PacCalendarModuleHandler> _pacCalendarModuleHandler;
		private Mock<ISessionFacade> _sessionFacade;
		private Mock<IProductService2> _productService2;

		[NUnitFw.SetUp]
		public void Setup()
		{
			_moduleHandlerAggregator = new Mock<ModuleHandlerAggregator>();
			_sessionFacade = new Mock<ISessionFacade>();
			_productService2 = new Mock<IProductService2>();

			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ISessionFacade>()).Returns(_sessionFacade.Object);
			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<IProductService2>()).Returns(_productService2.Object);

			_pacCalendarModuleHandler = new Mock<PacCalendarModuleHandler>(_moduleHandlerAggregator.Object){ CallBase = true };
		}


		[NUnitFw.Test]
		public void Initialize_Test()
		{

			var pacCalendarViewModelContext = new PacCalendarViewModelContext();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _pacCalendarModuleHandler.Object.Initialize(pacCalendarViewModelContext);
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void Update_Test()
		{

			var viewModel = new PacCalendarViewModel();
			var context = new PacCalendarViewModelContext();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			_pacCalendarModuleHandler.Object.Update(viewModel,context);
		}


		[NUnitFw.Test]
		public void AddBusinessDays_Test()
		{

			var dt = new DateTime();
			var nDays = new int();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _pacCalendarModuleHandler.Object.AddBusinessDays(dt,nDays);
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void GetBtHolidays_Test()
		{

			var startDate = new String();
			var endDate = new String();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _pacCalendarModuleHandler.Object.GetBtHolidays(startDate,endDate);
			//Assert.AreEqual("compareFrom", "compareTo");
		}


		[NUnitFw.Test]
		public void GetUKUIHolidayList_Test()
		{

			var UKHolidaysList = new List<UKHolidays>();
			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _pacCalendarModuleHandler.Object.GetUKUIHolidayList(UKHolidaysList);
			//Assert.AreEqual("compareFrom", "compareTo");
		}
	}
}
