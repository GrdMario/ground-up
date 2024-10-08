namespace GroundUp.Api.Application.Services
{
    using GroundUp.Api.Application.Models;
    using GroundUp.Api.Domain;
    using GroundUp.Api.Infrastructure.Database.Contracts;
    using GroundUp.Api.Services.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class MembershipService : IMembershipService
    {
        private readonly IUnitOfWork uow;

        public MembershipService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task CreateAsync(MembershipDto dto, CancellationToken cancellationToken)
        {
            var membership = new Membership(
                dto.ClientId,
                dto.From,
                dto.To,
                dto.SessionCount,
                dto.MembershipTypeId);

            this.uow.MembershipRepository.Add(membership);

            await this.uow.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<MembershipDto>> FilterMembershipsAsync(bool? isActive, bool? isCancelled, CancellationToken cancellationToken)
        {
            var memberships = await this.uow.MembershipRepository.FilterMembershipsAsync(isActive, isCancelled, cancellationToken);

            return memberships.Select(m => MembershipDto.FromMembership(m)).ToList();
        }

        public async Task<MembershipDto?> GetMembershipByDateAsync(Guid clientId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            var membership = await this.uow.MembershipRepository.GetMembershipByDateAsync(clientId, startDate, endDate, cancellationToken);

            if (membership == null)
            {
                return null;
            }

            return MembershipDto.FromMembership(membership);
        }

        public async Task<MembershipDto?> GetMembershipByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var membership = await this.uow.MembershipRepository.GetMembershipByIdAsync(id, cancellationToken);
            
            if (membership == null)
            {
                return null;
            }

            return MembershipDto.FromMembership(membership);
        }

        public async Task<List<MembershipDto>> GetMembershipForStartDateAsync(DateTime startDate, CancellationToken cancellationToken)
        {
            var members = await this.uow.MembershipRepository.GetMembershipsByStartDateAsync(startDate, cancellationToken);

            return members.Select(member => MembershipDto.FromMembership(member)).ToList();
        }

        public async Task<List<MembershipDto>> GetMembershipsByClientIdAsync(Guid clientId, CancellationToken cancellationToken)
        {
            var memberships = await this.uow.MembershipRepository.GetMembershipsByClientIdAsync(clientId, cancellationToken);

            return memberships.Select(m => MembershipDto.FromMembership(m)).ToList();
        }

        public async Task UpdateAsync(UpdateMembershipDto dto, CancellationToken cancellationToken)
        {
            var membership = await this.uow.MembershipRepository.GetMembershipByIdSafeAsync(dto.Id, cancellationToken);

            membership.Update(
                from: dto.From,
                to: dto.To,
                membershipTypeId: dto.MembershipTypeId,
                fronzenDate: dto.FrozenDate);

            this.uow.MembershipRepository.Update(membership);

            await this.uow.SaveChangesAsync(cancellationToken);
        }
    }
}
