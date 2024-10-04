namespace GroundUp.Api.Pages.Clients
{
    using GroundUp.Api.Application.Models;
    using GroundUp.Api.Models;
    using GroundUp.Api.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class CreateModel : PageModel
    {
        private readonly IClientService clientService;

        public CreateModel(IClientService clientService)
        {
            this.clientService = clientService;
        }

        [BindProperty]
        public ClientViewModel ClientViewModel { get; set; } = new()
        {
            DateOfBirth = DateTime.UtcNow
        };
       
        public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Page();
            }

            var clientDto = new CreateClientDto(
                this.ClientViewModel.FirstName,
                this.ClientViewModel.LastName,
                this.ClientViewModel.Email,
                this.ClientViewModel.PhoneNumber,
                this.ClientViewModel.DateOfBirth,
                this.ClientViewModel.Address,
                this.ClientViewModel.City,
                this.ClientViewModel.Description);

            await this.clientService.CreateAsync(clientDto, cancellationToken);

            return this.RedirectToPage("./Index");
        }
    }
}
