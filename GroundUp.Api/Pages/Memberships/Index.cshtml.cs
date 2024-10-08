namespace GroundUp.Api.Pages.Memberships
{
    using GroundUp.Api.Application.Models;
    using GroundUp.Api.Models;
    using GroundUp.Api.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class IndexModel : PageModel
    {
        private readonly IMembershipService membershipService;

        public bool IsActive { get; set; }

        public bool IsCancelled { get; set; }

        public List<MembershipViewModel> Memberships { get; set; } = [];

        public IndexModel(IMembershipService membershipService)
        {
            this.membershipService = membershipService;
        }

        public async Task<IActionResult> OnPostSearchAsync(bool isCancelled, bool isActive, CancellationToken cancellationToken)
        {
            bool? isCancelledNullable = null;
            bool? isActiveNullable = null;

            if (isCancelled)
            {
                isCancelledNullable = isCancelled;
            }

            if (isActive)
            {
                isActiveNullable = isActive;
            }

            var members = await this.membershipService.FilterMembershipsAsync(isActiveNullable, isCancelledNullable, cancellationToken);

            this.Map(members ?? []);
            return this.Page();
        }

        public async Task OnGetAsync(CancellationToken cancellationToken)
        {
            var members = await this.membershipService.FilterMembershipsAsync(null, null, cancellationToken);

            this.Map(members ?? []);
        }

        private void Map(List<MembershipDto> members)
        {
            this.Memberships = members
                .Select(membership => new MembershipViewModel()
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
                    FrozenDate = membership.FrozenDate,
                    MembershipType = new MembershipTypeViewModel()
                    {
                        Id = membership.MembershipType.Id,
                        Name = membership.MembershipType.Name,
                        Color = membership.MembershipType.Color,
                    },
                    MembershipClient = new MembershipClientViewModel()
                    {
                        FirstName = membership.Client.FirstName,
                        LastName = membership.Client.LastName,
                    }
                })
                .ToList();
        }
    }
}
