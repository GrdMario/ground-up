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

        public List<int> Values { get; set; } = [];

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
                            .OrderBy(ob => ob.Start.HasValue ? 0 : 1)
                            .ThenBy(ob => ob.Start)
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
                                Count = s.MembershipSessions
                                    .OrderBy(ob => ob.Start.HasValue ? 0 : 1)
                                    .ThenBy(ob => ob.Start)
                                    .ToList()
                                    .IndexOf(ms) + 1,
                            })
                            .ToList(),
                    SessionsLeft = s.MembershipSessions.Where(ms => ms.Start == null && ms.End == null && ms.IsCancelled == false).Count(),
                    FrozenDate = s.FrozenDate,
                    IsFrozen = s.FrozenDate != null,
                    DaysLeft = (s.To - DateTime.Now).Days > 0 ? Convert.ToInt32((s.To - DateTime.Now).Days) : 0,
                    MembershipType = new MembershipTypeViewModel()
                    {
                        Id = s.MembershipType.Id,
                        Name = s.MembershipType.Name,
                        Color = s.MembershipType.Color,
                    }
                })
                .ToList();

            this.ActiveMembership = this.Memberships.Where(s => s.From.Date <= DateTime.UtcNow.Date && s.To.Date >= DateTime.UtcNow.Date && s.FrozenDate == null).FirstOrDefault();

            if (this.ActiveMembership != null)
            {
                this.ActiveMembershipId = this.ActiveMembership.Id;

                var used = this.ActiveMembership.MembershipSessions.Where(s => s.Start != null && s.End != null && s.IsCancelled == false).Count();
                var free = this.ActiveMembership.MembershipSessions.Where(s => s.Start == null && s.End == null && s.IsCancelled == false).Count();
                var canceled = this.ActiveMembership.MembershipSessions.Where(s => s.IsCancelled).Count();

                this.Values = [used, free, canceled];
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
            var correctedStart = dto.Start;
            var correctedEnd = dto.End;

            if (correctedStart.HasValue)
            {
                correctedStart = new DateTime(
                correctedStart.Value.Year,
                correctedStart.Value.Month,
                correctedStart.Value.Day,
                correctedStart.Value.Hour,
                0,
                0);

                correctedEnd = correctedStart.Value.AddMinutes(55);
            }

            var update = new UpdateMembershipSessionDto()
            {
                Id = dto.SessionId,
                MembershipId = dto.MembershipId,
                Start = correctedStart,
                End = correctedEnd,
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
