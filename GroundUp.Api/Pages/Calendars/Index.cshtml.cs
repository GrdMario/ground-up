namespace GroundUp.Api.Pages.Calendars
{
    using GroundUp.Api.Models;
    using GroundUp.Api.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class IndexModel : PageModel
    {

        [BindProperty]
        public DateTime StartDate { get; set; }

        [BindProperty]
        public DateTime EndDate { get; set; }

        public List<CalendarItemViewModel> Items { get; set; } = [];

        public List<DateTime> Dates { get; set; } = [];

        private readonly ICalendarService calendarService;

        public IndexModel(ICalendarService calendarService)
        {
            this.calendarService = calendarService;
        }

        public async Task OnGetAsync(int? year, int? month, int? day, CancellationToken cancellationToken)
        {
            DateTime currentDate = DateTime.Now;

            if (year != null && month != null && day != null)
            {
                currentDate = new DateTime(year.Value, month.Value, day.Value);
            }

            int currentDayOfWeek = (int)currentDate.DayOfWeek;

            if (currentDayOfWeek == (int)DayOfWeek.Sunday)
            {
                currentDayOfWeek = 7;
            }

            this.StartDate = currentDate.AddDays(-currentDayOfWeek + (int)DayOfWeek.Monday);
            this.EndDate = this.StartDate.AddDays(6);

            for (int i = 0; i < 7; i++)
            {
                this.Dates.Add(this.StartDate.AddDays(i));
            }

            var calendarItems = await this.calendarService.GetCalendarItemsAsync(this.StartDate, this.EndDate, cancellationToken);

            this.Items = calendarItems
                .Select(s => new CalendarItemViewModel(
                    s.MembershipSessionId,
                    s.MembershipSessionStartDate,
                    s.MembershipSessionEndDate,
                    s.StartTime,
                    s.EndTime,
                    s.ClientId,
                    s.ClientName,
                    s.MembershipTypeColor,
                    s.MembershipTypeName,
                    s.IsCancelled))
                .ToList();
        }

        public IActionResult OnPostPreviousWeek(int year, int month, int day)
        {
            var previous = new DateTime(year, month, day).AddDays(-7);

            return this.RedirectToPage("/Calendars/Index", new { year = previous.Year, month = previous.Month, day = previous.Day });
        }

        public IActionResult OnPostNextWeek(int year, int month, int day)
        {
            var next = new DateTime(year, month, day).AddDays(7);

            return this.RedirectToPage("/Calendars/Index", new { year = next.Year, month = next.Month, day = next.Day });
        }
    }
}
