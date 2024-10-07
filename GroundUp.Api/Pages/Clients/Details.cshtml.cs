namespace GroundUp.Api.Pages.Clients
{
    using GroundUp.Api.Application.Contracts;
    using GroundUp.Api.Application.Models;
    using GroundUp.Api.Models;
    using GroundUp.Api.Services.Contracts;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class DetailsModel : PageModel
    {
        [BindProperty]
        public Guid Id { get; set; }

        [BindProperty]
        public ClientViewModel ClientViewModel { get; set; } = new();

        public List<MembershipViewModel> Memberships { get; set; } = [];

        public MembershipViewModel? ActiveMembership { get; set; }

        [BindProperty]
        public Guid? ActiveMembershipId { get; set; }

        private readonly IClientService clientService;
        private readonly IMembershipService membershipService;
        private readonly IMembershipSessionService membershipSessionService;

        public DetailsModel(
            IClientService clientService,
            IMembershipService membershipService,
            IMembershipSessionService membershipSessionService)
        {
            this.clientService = clientService;
            this.membershipService = membershipService;
            this.membershipSessionService = membershipSessionService;
        }

        public async Task OnGetAsync(Guid id, CancellationToken cancellationToken)
        {
            this.Id = id;
            var clientDto = await this.clientService.GetByIdAsync(id, cancellationToken);

            this.ClientViewModel = new ClientViewModel()
            {
                Id = clientDto.Id,
                FirstName = clientDto.FirstName,
                LastName = clientDto.LastName,
                Email = clientDto.Email,
                PhoneNumber = clientDto.PhoneNumber,
                DateOfBirth = clientDto.DateOfBirth,
                Address = clientDto.Address,
                City = clientDto.City,
                Description = clientDto.Description,
            };

            var members = await this.membershipService.GetMembershipsByClientIdAsync(id, cancellationToken);

            this.Memberships =
                members
                .Select(s => new MembershipViewModel()
                {
                    ClientId = s.ClientId,
                    From = s.From,
                    To = s.To,
                    Id = s.Id,
                    SessionCount = s.SessionCount,
                    MembershipSessions =
                        s.MembershipSessions
                            .Select(ms => new MembershipSessionViewModel()
                            {
                                SessionId = ms.Id,
                                Id = ms.Id,
                                Comment = ms.Comment,
                                End = ms.End,
                                IsCancelled = ms.IsCancelled,
                                MembershipId = ms.MembershipId,
                                ClientId = s.ClientId,
                                Start = ms.Start,
                                CreatedAt = ms.CreatedAt,
                                Count = s.MembershipSessions.IndexOf(ms) + 1,
                            })
                            .ToList(),
                    PaidDate = s.FrozenDate,
                    MembershipType = new MembershipTypeViewModel()
                    {
                        Id = s.MembershipType.Id,
                        Name = s.MembershipType.Name,
                        Color = s.MembershipType.Color,
                    }
                })
                .ToList();

            this.ActiveMembership = this.Memberships.Where(s => s.From.Date <= DateTime.UtcNow.Date && s.To.Date >= DateTime.UtcNow.Date).FirstOrDefault();

            if (this.ActiveMembership != null)
            {
                this.ActiveMembershipId = this.ActiveMembership.Id;
            }
        }

        public async Task<IActionResult> OnPostUpdateInfoAsync(CancellationToken cancellationToken)
        {
            if (!this.ModelState.IsValid)
            {
                await this.OnGetAsync(this.ClientViewModel.Id, cancellationToken);
                return this.Page();
            }

            var clientDto = new UpdateClientDto(
                this.ClientViewModel.Id,
                this.ClientViewModel.FirstName,
                this.ClientViewModel.LastName,
                this.ClientViewModel.Email,
                this.ClientViewModel.PhoneNumber,
                this.ClientViewModel.DateOfBirth,
                this.ClientViewModel.Address,
                this.ClientViewModel.City,
                this.ClientViewModel.Description);

            await this.clientService.UpdateAsync(clientDto, cancellationToken);

            return this.RedirectToPage("Details", new { id = clientDto.Id });
        }

        public async Task<IActionResult> OnPostUpdateMembershipSessionAsync(MembershipSessionViewModel dto, CancellationToken cancellationToken)
        {
            var update = new UpdateMembershipSessionDto()
            {
                Id = dto.SessionId,
                MembershipId = dto.MembershipId,
                Start = dto.Start,
                End = dto.End,
                IsCancelled = dto.IsCancelled,
                Comment = dto.Comment
            };

            await this.membershipSessionService.UpdateAsync(update, cancellationToken);

            return this.RedirectToPage("Details", new { id = dto.ClientId });
        }

        public async Task<IActionResult> OnPostDeleteMembershipSessionAsync(MembershipSessionViewModel dto, CancellationToken cancellationToken)
        {
            var update = new UpdateMembershipSessionDto()
            {
                Id = dto.SessionId,
                MembershipId = dto.MembershipId,
                Start = null,
                End = null,
            };

            await this.membershipSessionService.UpdateAsync(update, cancellationToken);

            return this.RedirectToPage("Details", new { id = this.Id });
        }

        public async Task<IActionResult> OnPostAddNewSessionAsync(CancellationToken cancellationToken)
        {
            var dto = new CreateMembershipSessionDto()
            {
                MembershipId = this.ActiveMembershipId!.Value,
                Start = null,
                End = null,
                Comment = null,
            };

            await this.membershipSessionService.CreateAsync(dto, cancellationToken);

            return this.RedirectToPage("Details", new { id = this.Id });
        }

        public IActionResult OnPostCreateMembership(Guid id)
        {
            return this.RedirectToPage("CreateMembership", new { clientId = id });
        }
    }
}
