namespace GroundUp.Api.Application.Contracts
{
    using GroundUp.Api.Application.Models;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMembershipSessionService
    {
        Task UpdateAsync(UpdateMembershipSessionDto dto, CancellationToken cancellationToken);

        Task CreateAsync(CreateMembershipSessionDto dto, CancellationToken cancellationToken);

        Task<MembershipSessionDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}
