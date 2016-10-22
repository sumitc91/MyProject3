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

namespace BT.SaaS.HD.Mobility.Modules.PAC
{
    public class PacCalendarModuleHandler : IModuleHandler<PacCalendarViewModel, PacCalendarViewModelContext>
    {
        public ModuleHandlerAggregator ModuleHandlerAggregator { get; private set; }

        public PacCalendarModuleHandler()
            : this(ModuleHandlerAggregator.Current)
        {
        }

        public PacCalendarModuleHandler(ModuleHandlerAggregator aggregator)
        {
            ModuleHandlerAggregator = aggregator;
            ModuleHandlerAggregator.RegisterType<ProductService2>();
            ModuleHandlerAggregator.RegisterType<SessionFacade>();
        }

        public PacCalendarViewModel Initialize(PacCalendarViewModelContext pacCalendarViewModelContext)
        {
            var PacCalendarViewModel = new PacCalendarViewModel();
            PacCalendarViewModel.SelectedDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            PacCalendarViewModel.MyStartDate = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            PacCalendarViewModel.MonthsCount = pacCalendarViewModelContext.CalendarCount;

            //Adding 29 days from current days exclusing Weekends.
            PacCalendarViewModel.MyEndDate = AddBusinessDays(DateTime.Now, 29).ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
            PacCalendarViewModel.Disableddates = GetBtHolidays(PacCalendarViewModel.MyStartDate, PacCalendarViewModel.MyEndDate);
            return PacCalendarViewModel;
        }

        public void Update(PacCalendarViewModel viewModel, PacCalendarViewModelContext context)
        {
            throw new NotImplementedException();
        }

        private DateTime AddBusinessDays(DateTime dt, int nDays)
        {
            int weeks = nDays / 5;
            nDays %= 5;
            while (dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday)
                dt = dt.AddDays(1);

            while (nDays-- > 0)
            {
                dt = dt.AddDays(1);
                if (dt.DayOfWeek == DayOfWeek.Saturday)
                    dt = dt.AddDays(2);
            }
            return dt.AddDays(weeks * 7);
        }

        private string[] GetBtHolidays(String startDate, String endDate)
        {
            string resellerid = ModuleHandlerAggregator.TryCreateOrGet<ISessionFacade>().CustomerData.ResellerID;
            List<UKHolidays> holidays = ModuleHandlerAggregator.TryCreateOrGet<IProductService2>().GetUKHolidays(resellerid); //UIUtilities.GetUKHolidays(Convert.ToInt32(resellerid));
            string[] holidayDates = GetUKUIHolidayList(holidays);
            return holidayDates;
        }

        private string[] GetUKUIHolidayList(List<UKHolidays> UKHolidaysList)
        {
            List<string> _holidayList = new List<string>();
            if (UKHolidaysList != null)
            {
                UKHolidaysList.ForEach(HL =>
                {
                    if (HL.HolidayDate != DateTime.MinValue)
                    {
                        _holidayList.Add(HL.HolidayDate.Value.ToString("dd-MM-yyyy"));
                    }
                });
            }
            return _holidayList.ToArray();
        }
    }
}