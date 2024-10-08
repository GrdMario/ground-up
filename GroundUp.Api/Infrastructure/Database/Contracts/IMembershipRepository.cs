namespace GroundUp.Api.Infrastructure.Database.Contracts
{
    using GroundUp.Api.Domain;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMembershipRepository
    {
        Task<List<Membership>> FilterMembershipsAsync(bool? isActive, bool? isCancelled, CancellationToken cancellationToken);

        Task<Membership> GetMembershipByIdSafeAsync(Guid id, CancellationToken cancellationToken);

        Task<Membership?> GetMembershipByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<List<Membership>> GetMembershipsByClientIdAsync(Guid clientId, CancellationToken cancellationToken);

        Task<Membership?> GetMembershipByDateAsync(Guid clientId, DateTime startDate, DateTime endDate, CancellationToken cancellationToken);

        Task<List<Membership>> GetMembershipsByStartDateAsync(DateTime startDate, CancellationToken cancellationToken);

        void Add(Membership membership);

        void Update(Membership membership);

        void Delete(Membership membership);
    }
}
