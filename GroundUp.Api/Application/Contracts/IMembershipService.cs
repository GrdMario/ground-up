namespace GroundUp.Api.Services.Contracts
{
    using GroundUp.Api.Application.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMembershipService
    {
        Task<List<MembershipDto>> GetMembershipsByClientIdAsync(Guid clientId, CancellationToken cancellationToken);

        Task<List<MembershipDto>> FilterMembershipsAsync(bool? isActive, CancellationToken cancellationToken);

        Task<MembershipDto?> GetMembershipByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<MembershipDto?> GetMembershipByDateAsync(Guid clientId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);

        Task<List<MembershipDto>> GetMembershipForStartDateAsync(DateTime startDate, CancellationToken cancellationToken);

        Task CreateAsync(MembershipDto dto, CancellationToken cancellationToken);

        Task UpdateAsync(UpdateMembershipDto dto, CancellationToken cancellationToken);
    }
}
