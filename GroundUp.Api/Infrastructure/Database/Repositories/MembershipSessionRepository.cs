namespace GroundUp.Api.Infrastructure.Database.Repositories
{
    using GroundUp.Api.Domain;
    using GroundUp.Api.Infrastructure.Database.Contracts;
    using GroundUp.Api.Infrastructure.Database.Internal;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class MembershipSessionRepository : IMembershipSessionRepository
    {
        private readonly DbSet<MembershipSession> membershipSessions;

        public MembershipSessionRepository(GroundUpContext context)
        {
            this.membershipSessions = context.Set<MembershipSession>();
        }

        public void Create(MembershipSession session)
        {
            this.membershipSessions.Add(session);
        }

        public void Delete(MembershipSession session)
        {
            this.membershipSessions.Remove(session);
        }

        public async Task<MembershipSession?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await this.membershipSessions.FindAsync(new object[] { id}, cancellationToken);
        }

        public async Task<MembershipSession> GetByIdSafeAsync(Guid id, CancellationToken cancellationToken)
        {
            return await this.GetByIdAsync(id, cancellationToken) ?? throw new Exception("Not found.");
        }

        public async Task<List<MembershipSession>> GetByStartAndEndDateAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            return await this.membershipSessions
                .Where(
                    s => s.Start != null
                        && s.End != null
                        && s.Start.Value.Date >= startDate.Date
                        && s.End.Value.Date <= endDate.Date)
                .Include(s => s.Membership)
                    .ThenInclude(s => s.MembershipType)
                .Include(s => s.Membership)
                    .ThenInclude(s => s.Client)
                .ToListAsync(cancellationToken);
        }

        public void Update(MembershipSession session)
        {
            this.membershipSessions.Update(session);
        }
    }
}
