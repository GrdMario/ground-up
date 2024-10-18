namespace GroundUp.Api.Pages.Organizations
{
    using GroundUp.Api.Application.Contracts;
    using GroundUp.Api.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using static System.Net.Mime.MediaTypeNames;

    public class IndexModel : PageModel
    {
        private readonly ICalendarService calendarService;
        private readonly IMembershipTypeService membershipTypeService;
        private readonly IMembershipService membershipService;
        private readonly IClientService clientService;

        public int ClientsCount { get; set; }

        public int MembershipsCount { get; set; }

        public int TypesCount { get; set; }

        public int ClientCountThisWeek { get; set; }

        public int SessionCountThisWeek { get; set; }

        public int PassedSessionCountThisWeek { get; set; }

        public int CancelledSessionCountThisWeek { get; set; }

        public List<string> Months = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];

        public List<int> ClientsData = [];

        public List<int> MembershipData = [];

        public List<SessionItemView> SessionItems = [];

        public List<string> TodayTypes = [];

        public List<int> TodayTypesData = [];

        public IndexModel(IMembershipService membershipService, IClientService clientService, IMembershipTypeService membershipTypeService, ICalendarService calendarService)
        {
            this.membershipService = membershipService;
            this.clientService = clientService;
            this.membershipTypeService = membershipTypeService;
            this.calendarService = calendarService;
        }

        public async Task OnGet(CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            var start = new DateTime(now.Year, 1, 1);
            var end = new DateTime(now.Year, 12, 31);

            var beginningOfWeek = now.AddDays(-(int)now.DayOfWeek + (int)DayOfWeek.Monday);
            var endOfWeek = beginningOfWeek.AddDays(6);

            var clients = await this.clientService.GetClientsByStartDateAndEndDateAsync(start, end, cancellationToken);
            var memberships = await this.membershipService.GetMembershipsBetweenStartDateAndEndDate(start, end, cancellationToken);
            var types = await this.membershipTypeService.GetAllAsync(cancellationToken);

            var calendarItems = await this.calendarService.GetCalendarItemsAsync(beginningOfWeek, endOfWeek, cancellationToken);

            var groupedClients = clients
                .GroupBy(gb => gb.CreatedAt.Date.Month)
                .Select(s => new
                {
                    Month = s.Key,
                    Data = s.Count()
                })
                .ToList();

            var groupedMemberships = memberships
                .GroupBy(gb => gb.From.Date.Month)
                .Select(s => new
                {
                    Month = s.Key,
                    Data = s.Count()
                })
                .ToList();

            this.ClientsCount = clients.Count;
            this.MembershipsCount = memberships.Count;
            this.TypesCount = types.Count;

            for (int i = 0; i < this.Months.Count; i++)
            {
                var numberOfClientsInMonth = groupedClients
                    .OrderBy(ob => ob.Month)
                    .Where(s => s.Month == i + 1).Select(s => s.Data).FirstOrDefault();

                this.ClientsData.Add(numberOfClientsInMonth);

                var numberOfMembershipsInMonth = groupedMemberships
                    .OrderBy(ob => ob.Month)
                    .Where(s => s.Month == i + 1).Select(s => s.Data)
                    .FirstOrDefault();

                this.MembershipData.Add(numberOfMembershipsInMonth);
            }

            this.ClientCountThisWeek = calendarItems.DistinctBy(s => s.ClientId).Count();
            this.SessionCountThisWeek = calendarItems.Count;
            this.PassedSessionCountThisWeek = calendarItems.Where(s => s.MembershipSessionEndDate <= now).Count();
            this.CancelledSessionCountThisWeek = calendarItems.Where(s => s.IsCancelled == true).Count();
            this.TodayTypes = types.Select(s => s.Name).ToList();
            this.SessionItems = calendarItems
                .Where(s => s.MembershipSessionStartDate.Date >= now.Date && s.MembershipSessionEndDate.Date <= now.Date)
                .Select(s => new SessionItemView(s.ClientName, s.MembershipTypeColor, s.IsCancelled, s.MembershipSessionStartDate, s.MembershipSessionEndDate))
                .ToList();

            var groupedTypes =  calendarItems
                .Where(s => s.MembershipSessionStartDate.Date >= now.Date && s.MembershipSessionEndDate.Date <= now.Date)
                .GroupBy(gb => gb.MembershipTypeName)
                .Select(s => new
                {
                    Type = s.Key,
                    Data = s.Count()
                })
                .ToList();
            
            foreach(var item in types)
            {
                var numberOfSessionOfType = groupedTypes.Where(s =>s.Type == item.Name).Select(s => s.Data).FirstOrDefault();

                this.TodayTypesData.Add(numberOfSessionOfType);
            }
        }
    }

    public record SessionItemView(string Name, string Color, bool IsCancelled, DateTime Start, DateTime End);

    public record TypesItemView(string Name);
}
