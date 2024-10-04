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

    public class CreateMembershipModel : PageModel
    {
        [BindProperty]
        public CreateMembershipViewModel CreateMembershipViewModel { get; set; } = new CreateMembershipViewModel()
        {
            From = DateTime.UtcNow,
            To = DateTime.UtcNow,
        };

        public List<SelectListItem> MembershipTypes { get; set; } = [];

        [BindProperty]
        public string? ClientName { get; set; }

        public Guid ClientId { get; set; }

        private readonly IMembershipTypeService membershipTypeService;
        private readonly IMembershipService membershipService;

        public CreateMembershipModel(
            IMembershipTypeService membershipTypeService,
            IMembershipService membershipService)
        {
            this.membershipTypeService = membershipTypeService;
            this.membershipService = membershipService;
        }

        public async Task OnGetAsync(string name, Guid id, CancellationToken cancellationToken)
        {
            var types = await this.membershipTypeService.GetAllAsync(cancellationToken);

            this.MembershipTypes = types.Select(t => new SelectListItem()
            {
                Value = t.Id.ToString(),
                Text = t.Name,
            })
            .ToList();

            this.ClientName = name;
            this.ClientId = id;
            this.CreateMembershipViewModel.ClientId = this.ClientId;
        }

        public async Task<IActionResult> OnPostCreateAsync(CancellationToken cancellationToken)
        {
            if (this.CreateMembershipViewModel.From > this.CreateMembershipViewModel.To)
            {
                this.ModelState.AddModelError("CreateMembershipViewModel.From", "Membership needs to start before it ends. Check your 'Start Date' and 'End Date' fields.");
            }

            var existingMembership = await this.membershipService.GetMembershipByDateAsync(this.CreateMembershipViewModel.ClientId, this.CreateMembershipViewModel.From, cancellationToken);

            if (existingMembership != null)
            {
                this.ModelState.AddModelError("CreateMembershipViewModel.From", "This client already has a membership that overlaps with selected 'Start Date'.");
            }

            if (!this.ModelState.IsValid)
            {
                var types = await this.membershipTypeService.GetAllAsync(cancellationToken);
                var clientId = this.ClientId;

                this.MembershipTypes = types.Select(t => new SelectListItem()
                {
                    Value = t.Id.ToString(),
                    Text = t.Name,
                })
                .ToList();

                this.ClientId = this.CreateMembershipViewModel.ClientId;

                return this.Page();
            }

            var membershipDto = new MembershipDto()
            {
                ClientId = this.CreateMembershipViewModel.ClientId,
                From = this.CreateMembershipViewModel.From,
                To = this.CreateMembershipViewModel.To,
                SessionCount = this.CreateMembershipViewModel.SessionCount,
                MembershipTypeId = this.CreateMembershipViewModel.MembershipTypeId
            };

            await this.membershipService.CreateAsync(membershipDto, cancellationToken);

            return this.RedirectToPage("/Clients/Details", new { id = this.CreateMembershipViewModel.ClientId });
        }
    }
}
