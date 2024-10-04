namespace GroundUp.Api.Pages.MembershipsTypes
{
    using GroundUp.Api.Application.Contracts;
    using GroundUp.Api.Models;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class IndexModel : PageModel
    {
        private readonly IMembershipTypeService membershipTypeService;

        public List<MembershipTypeViewModel> MembershipTypes { get; set; } = new();

        public IndexModel(IMembershipTypeService membershipService)
        {
            this.membershipTypeService = membershipService;
        }

        public async Task OnGet(CancellationToken cancellationToken)
        {
            var membershipTypes = await this.membershipTypeService.GetAllAsync(cancellationToken);

            this.MembershipTypes = membershipTypes.Select(membershipType => new MembershipTypeViewModel()
            {
                Id = membershipType.Id,
                Name = membershipType.Name,
                Color = membershipType.Color
            }).ToList();
        }
    }
}
