namespace GroundUp.Api.Services.Contracts
{
    using GroundUp.Api.Application.Models;
    using GroundUp.Api.Domain;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMembershipService
    {
        Task<List<MembershipDto>> GetMembershipsByClientIdAsync(Guid clientId, CancellationToken cancellationToken);

        Task<List<MembershipDto>> FilterMembershipsAsync(bool? isActive, bool? isCancelled, CancellationToken cancellationToken);

        Task<MembershipDto?> GetMembershipByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<MembershipDto?> GetMembershipByDateAsync(Guid clientId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);

        Task<List<MembershipDto>> GetMembershipForStartDateAsync(DateTime startDate, CancellationToken cancellationToken);

        Task<List<MembershipDto>> GetMembershipsBetweenStartDateAndEndDate(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);

        Task CreateAsync(MembershipDto dto, CancellationToken cancellationToken);

        Task UpdateAsync(UpdateMembershipDto dto, CancellationToken cancellationToken);
    }
}
