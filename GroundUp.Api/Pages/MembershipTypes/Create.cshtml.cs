namespace GroundUp.Api.Pages.MembershipTypes
{
    using GroundUp.Api.Application.Contracts;
    using GroundUp.Api.Application.Models;
    using GroundUp.Api.Models;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateModel : PageModel
    {
        [BindProperty]
        public CreateMembershipTypeViewModel CreateMembershipTypeViewModel { get; set; } = default!;

        private readonly IMembershipTypeService membershipTypeService;

        public CreateModel(IMembershipTypeService membershipTypeService)
        {
            this.membershipTypeService = membershipTypeService;
        }

        public async Task<IActionResult> OnPostCreateAsync(CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                return this.Page();
            }

            var create = new MembershipTypeDto()
            {
                Id = Guid.NewGuid(),
                Name = this.CreateMembershipTypeViewModel.Name,
                Color = this.CreateMembershipTypeViewModel.Color,
            };

            await this.membershipTypeService.AddAsync(create, cancellationToken);

            return this.RedirectToPage("/MembershipTypes/Index");
        }
    }
}
