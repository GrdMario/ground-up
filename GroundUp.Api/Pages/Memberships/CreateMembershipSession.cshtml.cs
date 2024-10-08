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

        public List<MembershipDto> MembersWithMembership { get; set; } = new List<MembershipDto>();

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
                .Where(s => s.FrozenDate == null)
                .Where(s => s.MembershipSessions.Any(s => s.Start == null && s.End == null))
                .GroupBy(gb => gb.ClientId)
                .SelectMany(s => s)
                .Select(s => new SelectListItem()
                {
                    Value = s.Id.ToString(),
                    Text = $"{s.Client.FirstName} {s.Client.LastName}"
                })
                .ToList();

            this.MembersWithMembership =
                activeMembers
                .Where(s => s.MembershipSessions.Any(ms => ms.Start == null && ms.End == null))
                .ToList();

            this.Create.Start = from;
            this.Create.End = to;
        }

        public async Task<IActionResult> OnPostUpdateAsync(CancellationToken cancellationToken)
        {
            var activeMembers = await this.membershipService.GetMembershipForStartDateAsync(this.From, cancellationToken);

            var existingSession = activeMembers
                .Where(s => s.Id == this.Create.MembershipId)
                .Select(s => s.MembershipSessions.Where(s => s.Start == null && s.End == null).FirstOrDefault())
                .FirstOrDefault();

            if (existingSession == null)
            {
                this.ModelState.AddModelError("Create.MembershipId", "This client used all of his sessions.");
            }

            if (!ModelState.IsValid)
            {
                this.Year = this.Year;
                this.Month = this.Month;
                this.Day = this.Day;
                this.Hour = this.Hour;

                var from = new DateTime(this.Year, this.Month, this.Day, this.Hour, 0, 0);
                var to = new DateTime(this.Year, this.Month, this.Day, this.Hour, 59, 0);

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

                this.MembersWithMembership =
                    activeMembers
                    .Where(s => s.MembershipSessions.Any(ms => ms.Start == null && ms.End == null))
                    .ToList();

                this.From = from;

                return Page();
            }

            var model = new UpdateMembershipSessionDto()
            {
                Id = existingSession!.Id,
                MembershipId = existingSession!.MembershipId,
                Start = this.Create.Start,
                End = this.Create.End,
                Comment = this.Create.Comment,
                IsCancelled = existingSession!.IsCancelled
            };

            await this.membershipSessionService.UpdateAsync(model, cancellationToken);

            return RedirectToPage("/Index", new { year = this.Year, month = this.Month, day = this.Day});
        }
    }
}
