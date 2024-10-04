namespace GroundUp.Api.Infrastructure.Database.Contracts
{
    using GroundUp.Api.Domain;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMembershipSessionRepository
    {
        Task<MembershipSession?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<List<MembershipSession>> GetByStartAndEndDateAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);

        public Task<MembershipSession> GetByIdSafeAsync(Guid id, CancellationToken cancellationToken);

        public void Update(MembershipSession session);

        public void Delete(MembershipSession session);

        public void Create(MembershipSession session);
    }
}
