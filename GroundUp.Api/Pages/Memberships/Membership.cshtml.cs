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

    public class MembershipModel : PageModel
    {
        private readonly IMembershipService membershipService;
        private readonly IMembershipSessionService membershipSessionService;
        private readonly IMembershipTypeService membershipTypeService;

        [BindProperty]
        public UpdateMembershipViewModel Update { get; set; } = new();

        public List<SelectListItem> MembershipTypes { get; set; } = [];

        [BindProperty]
        public MembershipViewModel? MembershipViewModel { get; set; }

        [BindProperty]
        public Guid MembershipId { get; set; }

        public MembershipModel(
            IMembershipService membershipService,
            IMembershipSessionService membershipSessionService,
            IMembershipTypeService membershipTypeService)
        {
            this.membershipService = membershipService;
            this.membershipSessionService = membershipSessionService;
            this.membershipTypeService = membershipTypeService;
        }

        public async Task OnGetAsync(Guid id, CancellationToken cancellationToken)
        {
            this.MembershipId = id;

            var membership = await this.membershipService.GetMembershipByIdAsync(id, cancellationToken);
            var types = await this.membershipTypeService.GetAllAsync(cancellationToken);

            if (membership == null)
            {
                return;
            }

            this.MembershipTypes = types
                .Select(mt => new SelectListItem()
                {
                    Text = mt.Name,
                    Value = mt.Id.ToString(),
                })
                .ToList();

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
                FrozenDate = membership.FrozenDate,
                MembershipType = new MembershipTypeViewModel()
                {
                    Id = membership.MembershipType.Id,
                    Name = membership.MembershipType.Name,
                }
            };

            this.Update = new UpdateMembershipViewModel()
            {
                Id = membership.Id,
                ClientId = this.MembershipViewModel.ClientId,
                From = this.MembershipViewModel.From,
                MembershipTypeId = membership.MembershipType.Id,
                SessionCount = membership.SessionCount,
                FrozenDate = membership.FrozenDate,
            };
        }

        public async Task<IActionResult> OnPostUpdateMembershipAsync(CancellationToken cancellationToken)
        {
            var update = new UpdateMembershipDto()
            {
                Id = this.Update.Id,
                From = this.Update.From,
                To = this.Update.From.AddDays(32),
                MembershipTypeId = this.Update.MembershipTypeId,
                FrozenDate = this.Update.FrozenDate,
            };

            await Task.FromResult(1);
            return this.RedirectToPage("Membership", new { id = this.MembershipId });
        }

        public async Task<IActionResult> OnPostFreezeMembershipAsync(CancellationToken cancellationToken)
        {
            var existingMembership = await this.membershipService.GetMembershipByIdAsync(this.Update.Id, cancellationToken);

            if (existingMembership == null)
            {
                return this.RedirectToPage("Membership", new { id = this.MembershipId });
            }

            var update = new UpdateMembershipDto()
            {
                Id = existingMembership.Id,
                From = existingMembership.From,
                To = existingMembership.To,
                SessionCount = existingMembership.SessionCount,
                MembershipTypeId = existingMembership.MembershipType.Id,
                FrozenDate = DateTime.Now
            };

            await this.membershipService.UpdateAsync(update, cancellationToken);
            return this.RedirectToPage("Membership", new { id = this.MembershipId });
        }

        public async Task<IActionResult> OnPostUnfreezeMembershipAsync(CancellationToken cancellationToken)
        {
            var existingMembership = await this.membershipService.GetMembershipByIdAsync(this.Update.Id, cancellationToken);

            if (existingMembership == null)
            {
                return this.RedirectToPage("Membership", new { id = this.MembershipId });
            }

            var spentTimespan = existingMembership.FrozenDate - existingMembership.From;

            if (spentTimespan == null)
            {
                this.ModelState.AddModelError("Update.StartDate", "Unable to unfreeze membership since there is no difference between Start and Freeze date.");

                return this.RedirectToPage("Membership", new { id = this.MembershipId });
            }

            var differenceLeft = 32 - spentTimespan.Value.Days;

            var update = new UpdateMembershipDto()
            {
                Id = existingMembership.Id,
                From = existingMembership.From,
                To = DateTime.Now.AddDays(differenceLeft),
                SessionCount = existingMembership.SessionCount,
                MembershipTypeId = existingMembership.MembershipType.Id,
                FrozenDate = null
            };

            await this.membershipService.UpdateAsync(update, cancellationToken);
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
    }
}
