namespace WebApplication1.Pages
{
    using GroundUp.Api.Application.Contracts;
    using GroundUp.Api.Infrastructure.Database.Contracts;
    using GroundUp.Api.Services.Contracts;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class IndexModel : PageModel
    {

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public List<CalendarItemViewModel> Items { get; set; } = new();

        public List<DateTime> Dates { get; set; } = new();

        private readonly ICalendarService calendarService;

        public IndexModel(ICalendarService calendarService)
        {
            this.calendarService = calendarService;
        }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            DateTime currentDate = DateTime.Now;
            this.StartDate = currentDate.AddDays(-(int)currentDate.DayOfWeek + (int)DayOfWeek.Monday);
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
    }

    public record CalendarItemViewModel(
        Guid MembershipSessionId,
        DateTime MembershipSessionStartDate,
        DateTime MembershipSessionEndDate,
        int StartTime,
        int EndTime,
        Guid ClientId,
        string ClientName,
        string MembershipTypeColor,
        string MembershipTypeName,
        bool IsCancelled);
}
