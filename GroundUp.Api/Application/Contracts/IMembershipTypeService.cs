namespace GroundUp.Api.Application.Contracts
{
    using GroundUp.Api.Application.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMembershipTypeService
    {
        Task<MembershipTypeDto> GetMembershipTypeByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<List<MembershipTypeDto>> GetAllAsync(CancellationToken cancellationToken);

        Task AddAsync(MembershipTypeDto entity, CancellationToken cancellationToken);

        Task UpdateAsync(MembershipTypeDto entity, CancellationToken cancellationToken);
    }
}
