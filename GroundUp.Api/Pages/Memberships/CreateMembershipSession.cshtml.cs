namespace GroundUp.Api.Pages.Memberships
{
    using GroundUp.Api.Application.Contracts;
    using GroundUp.Api.Application.Models;
    using GroundUp.Api.Models;
    using GroundUp.Api.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateMembershipSessionModel : PageModel
    {

        [BindProperty]
        public CreateMembershipSessionViewModel Create { get; set; } = new();

        [BindProperty]
        public int Year { get; set; }

        [BindProperty]
        public int Month { get; set; }

        [BindProperty]
        public int Day { get; set; }

        [BindProperty]
        public int Hour { get; set; }

        [BindProperty]
        public DateTime From { get; set; }

        public List<SelectListItem> Members { get; set; } = [];

        private readonly IMembershipService membershipService;
        private readonly IMembershipSessionService membershipSessionService;
        public CreateMembershipSessionModel(IMembershipService membershipService, IMembershipSessionService membershipSessionService)
        {
            this.membershipService = membershipService;
            this.membershipSessionService = membershipSessionService;
        }

        public async Task OnGetAsync(int year, int month, int day, int hour, CancellationToken cancellationToken)
        {
            var from = new DateTime(year, month, day, hour, 0, 0);
            var to = new DateTime(year, month, day, hour, 59, 0);

            this.Year = year;
            this.Month = month;
            this.Day = day;
            this.Hour = hour;

            this.From = from;

            var activeMembers = await this.membershipService.GetMembershipForStartDateAsync(from, cancellationToken);

            this.Members =
                activeMembers
                .GroupBy(gb => gb.ClientId)
                .SelectMany(s => s)
                .Select(s => new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.Client.FirstName} {s.Client.LastName}"
                })
                .ToList();
            this.Create.Start = from;
            this.Create.End = to;
        }

        public async Task<IActionResult> OnPostUpdateAsync(CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                this.Year = this.Year;
                this.Month = this.Month;
                this.Day = this.Day;
                this.Hour = this.Hour;

                var from = new DateTime(this.Year, this.Month, this.Day, this.Hour, 0, 0);
                var to = new DateTime(this.Year, this.Month, this.Day, this.Hour, 59, 0);

                var activeMembers = await this.membershipService.GetMembershipForStartDateAsync(from, cancellationToken);

                this.Members =
                    activeMembers
                    .GroupBy(gb => gb.ClientId)
                    .SelectMany(s => s)
                    .Select(s => new SelectListItem()
                    {
                        Value = s.Id.ToString(),
                        Text = $"{s.Client.FirstName} {s.Client.LastName}"
                    })
                    .ToList();

                this.From = from;

                return Page();
            }

            var dto = new CreateMembershipSessionDto()
            {
                MembershipId = this.Create.MembershipId,
                Start = this.Create.Start,
                End = this.Create.End,
                Comment = this.Create.Comment
            };

            await this.membershipSessionService.CreateAsync(dto, cancellationToken);
            // Call a service to save the session (for example)
            //await this.membershipService.SaveMembershipSessionAsync(dto, cancellationToken);

            return RedirectToPage("/Index");
        }
    }
}
