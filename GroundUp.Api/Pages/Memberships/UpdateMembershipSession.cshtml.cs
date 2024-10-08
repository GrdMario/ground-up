namespace GroundUp.Api.Pages.Memberships
{
    using GroundUp.Api.Application.Contracts;
    using GroundUp.Api.Application.Models;
    using GroundUp.Api.Models;
    using GroundUp.Api.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class UpdateModel : PageModel
    {
        [BindProperty]
        public UpdateMembershipSessionViewModel Update { get; set; } = default!;

        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        private readonly IMembershipSessionService membershipSessionService;

        private readonly IClientService clientService;

        public UpdateModel(
            IMembershipSessionService membershipSessionService,
            IClientService clientService)
        {
            this.membershipSessionService = membershipSessionService;
            this.clientService = clientService;
        }

        public async Task OnGet(Guid id, Guid clientId, CancellationToken cancellationToken)
        {
            var client = await this.clientService.GetByIdAsync(clientId, cancellationToken);

            var membershipSession = await this.membershipSessionService.GetByIdAsync(id, cancellationToken);

            this.FirstName = client.FirstName;
            this.LastName = client.LastName;

            this.Update = new UpdateMembershipSessionViewModel()
            {
                Id = membershipSession.Id,
                MembershipId = membershipSession.Id,
                IsCancelled = membershipSession.IsCancelled,
                Start = membershipSession.Start,
                End = membershipSession.End,
                Comment = membershipSession.Comment,
            };
        }

        public async Task<IActionResult> OnPostUpdateAsync(CancellationToken cancellationToken)
        {
            var model = new UpdateMembershipSessionDto()
            {
                Id = this.Update.Id,
                MembershipId = this.Update.MembershipId,
                Start = this.Update.Start,
                End = this.Update.End,
                Comment = this.Update.Comment,
                IsCancelled = this.Update.IsCancelled,
            };

            await this.membershipSessionService.UpdateAsync(model, cancellationToken);

            return this.RedirectToPage("/Index", new { year = this.Update.Start!.Value.Year, month = this.Update.Start!.Value.Month, day = this.Update.Start!.Value.Day });
        }

        public async Task<IActionResult> OnPostDeleteAsync(CancellationToken cancellationToken)
        {
            var model = new UpdateMembershipSessionDto()
            {
                Id = this.Update.Id,
                MembershipId = this.Update.MembershipId,
                Start = null,
                End = null,
                Comment = this.Update.Comment,
                IsCancelled = this.Update.IsCancelled,
            };

            await this.membershipSessionService.UpdateAsync(model, cancellationToken);
            return this.RedirectToPage("/Index", new { year = this.Update.Start!.Value.Year, month = this.Update.Start!.Value.Month, day = this.Update.Start!.Value.Day });
            return this.RedirectToPage("/Index");
        }
    }
}
