namespace GroundUp.Api.Pages.Memberships
{
    using GroundUp.Api.Application.Contracts;
    using GroundUp.Api.Application.Models;
    using GroundUp.Api.Models;
    using GroundUp.Api.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class MembershipModel : PageModel
    {
        private readonly IMembershipService membershipService;
        private readonly IMembershipSessionService membershipSessionService;

        [BindProperty]
        public MembershipViewModel? MembershipViewModel { get; set; }

        [BindProperty]
        public Guid MembershipId { get; set; }

        public MembershipModel(IMembershipService membershipService, IMembershipSessionService membershipSessionService)
        {
            this.membershipService = membershipService;
            this.membershipSessionService = membershipSessionService;
        }

        public async Task OnGetAsync(Guid id, CancellationToken cancellationToken)
        {
            this.MembershipId = id;

            var membership = await this.membershipService.GetMembershipByIdAsync(id, cancellationToken);

            if (membership == null)
            {
                return;
            }

            this.MembershipViewModel = new MembershipViewModel()
            {
                ClientId = membership.ClientId,
                From = membership.From,
                To = membership.To,
                Id = membership.Id,
                SessionCount = membership.SessionCount,
                MembershipSessions =
                        membership.MembershipSessions
                            .Select(ms => new MembershipSessionViewModel()
                            {
                                SessionId = ms.Id,
                                Id = ms.Id,
                                Comment = ms.Comment,
                                End = ms.End,
                                IsCancelled = ms.IsCancelled,
                                MembershipId = ms.MembershipId,
                                ClientId = membership.ClientId,
                                Start = ms.Start,
                                CreatedAt = ms.CreatedAt,
                                Count = membership.MembershipSessions.IndexOf(ms) + 1,
                            })
                            .ToList(),
                PaidDate = membership.FrozenDate,
                MembershipType = new MembershipTypeViewModel()
                {
                    Id = membership.MembershipType.Id,
                    Name = membership.MembershipType.Name,
                }
            };
        }

        public async Task<IActionResult> OnPostUpdateMembershipSessionAsync(MembershipSessionViewModel dto, CancellationToken cancellationToken)
        {
            var update = new UpdateMembershipSessionDto()
            {
                Id = dto.SessionId,
                MembershipId = this.MembershipId,
                Start = dto.Start,
                End = dto.End,
                IsCancelled = dto.IsCancelled,
                Comment = dto.Comment
            };

            await this.membershipSessionService.UpdateAsync(update, cancellationToken);

            return this.RedirectToPage("Membership", new { id = this.MembershipId });
        }

        public async Task<IActionResult> OnPostDeleteMembershipSessionAsync(MembershipSessionViewModel dto, CancellationToken cancellationToken)
        {
            var update = new UpdateMembershipSessionDto()
            {
                Id = dto.SessionId,
                MembershipId = this.MembershipId,
                Start = null,
                End = null,
                IsCancelled = dto.IsCancelled,
                Comment = dto.Comment
            };

            await this.membershipSessionService.UpdateAsync(update, cancellationToken);

            return this.RedirectToPage("Membership", new { id = this.MembershipId });
        }

        public async Task<IActionResult> OnPostAddNewSessionAsync(CancellationToken cancellationToken)
        {
            var dto = new CreateMembershipSessionDto()
            {
                MembershipId = this.MembershipId
            };

            await this.membershipSessionService.CreateAsync(dto, cancellationToken);

            return this.RedirectToPage("Membership", new { id = this.MembershipId });
        }
    }
}
