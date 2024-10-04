namespace GroundUp.Api.Infrastructure.Database.Contracts
{
    using GroundUp.Api.Domain;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMembershipTypeRepository
    {
        Task<List<MembershipType>> GetAllAsync(CancellationToken cancellationToken);

        Task<MembershipType> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        void Add(MembershipType entity);

        void Update(MembershipType entity);
    }
}
