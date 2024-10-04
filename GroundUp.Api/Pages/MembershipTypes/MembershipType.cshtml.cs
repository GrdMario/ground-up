namespace GroundUp.Api.Pages.MembershipTypes
{
    using GroundUp.Api.Application.Contracts;
    using GroundUp.Api.Application.Models;
    using GroundUp.Api.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class MembershipTypeModel : PageModel
    {
        [BindProperty]
        public UpdateMembershipTypeViewModel UpdateMembershipTypeViewModel { get; set; } = default!;

        private readonly IMembershipTypeService membershipTypeService;

        public MembershipTypeModel(IMembershipTypeService membershipTypeService)
        {
            this.membershipTypeService = membershipTypeService;
        }

        public async Task OnGet(Guid id, CancellationToken cancellationToken)
        {
            var membershipType = await this.membershipTypeService.GetMembershipTypeByIdAsync(id, cancellationToken);

            this.UpdateMembershipTypeViewModel = new UpdateMembershipTypeViewModel()
            {
                Id = membershipType.Id,
                Name = membershipType.Name,
                Color = membershipType.Color,
            };
        }

        public async Task<IActionResult> OnPostUpdateAsync(CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return this.Page();
            }

            var update = new MembershipTypeDto()
            {
                Id = this.UpdateMembershipTypeViewModel.Id,
                Name = this.UpdateMembershipTypeViewModel.Name,
                Color = this.UpdateMembershipTypeViewModel.Color,
            };

            await this.membershipTypeService.UpdateAsync(update, cancellationToken);

            return this.RedirectToPage("/MembershipTypes/Index");
        }
    }
}
