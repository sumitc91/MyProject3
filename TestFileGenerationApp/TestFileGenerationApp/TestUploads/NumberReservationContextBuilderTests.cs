using BT.SaaS.Core.Entity.Utils.Facades;
using BT.SaaS.Core.UI.Framework.BusinessEntities;
using BT.SaaS.HD.Mobility.Utility;
using BT.SaaS.HD.UI.Common.ModuleHandlerFactory;
using BT.SaaS.HD.UI.Common.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Moq;
using NUnitFw = NUnit.Framework;

namespace BT.SaaS.HD.Mobility.Tests.ContextBuilder.Shared
{
	[NUnitFw.TestFixture]
	public class NumberReservationContextBuilderTest
	{
		private Mock<ModuleHandlerAggregator> _moduleHandlerAggregator;
		private NumberReservationContextBuilder _numberReservationContextBuilder;
		private Mock<ISessionFacade> _sessionFacade;

		[NUnitFw.SetUp]
		public void Setup()
		{
			_moduleHandlerAggregator = new Mock<ModuleHandlerAggregator>();
			_sessionFacade = new Mock<ISessionFacade>();

			_moduleHandlerAggregator.Setup(a => a.TryCreateOrGet<ISessionFacade>()).Returns(_sessionFacade.Object);

			_numberReservationContextBuilder = new NumberReservationContextBuilder(_moduleHandlerAggregator.Object);
		}


		[NUnitFw.Test]
		public void BuildMNUMRequest_Test()
		{

			//_mockObj.Setup(a => a.methodName(It.IsAny<inputParamType>())).Returns(returnObj);
			var response = _numberReservationContextBuilder.BuildMNUMRequest();
			//Assert.AreEqual("compareFrom", "compareTo");
		}
	}
}
