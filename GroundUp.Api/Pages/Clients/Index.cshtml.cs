namespace GroundUp.Api.Pages.Clients
{
    using GroundUp.Api.Application.Models;
    using GroundUp.Api.Domain;
    using GroundUp.Api.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class IndexModel : PageModel
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public List<ClientDto> Clients = new();

        private readonly IClientService clientService;

        public IndexModel(IClientService clientService)
        {
            this.clientService = clientService;
        }

        public async Task OnGetAsync(string firstName, string lastName, CancellationToken cancellationToken)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Clients = await this.clientService.GetClientsAsync(this.FirstName, this.LastName, 0, 10, cancellationToken);
        }

        public async Task<IActionResult> OnPostFirstAsync(string firstName, string lastName, CancellationToken cancellationToken)
        {
            this.FirstName = firstName;
            this.LastName = lastName;

            this.Clients = await this.clientService.GetClientsAsync(this.FirstName, this.LastName, 0, 10, cancellationToken);
            return this.Page();
        }

        public IActionResult OnPostSecond()
        {
            return this.Redirect("/Clients/Create");
        }
    }
}
