﻿namespace GroundUp.Api.Infrastructure.Database.Repositories
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

    internal sealed class MembershipRepository : IMembershipRepository
    {
        private readonly DbSet<Membership> memberships;

        public MembershipRepository(GroundUpContext context)
        {
            this.memberships = context.Set<Membership>();
        }

        public void Add(Membership membership)
        {
            this.memberships.Add(membership);
        }

        public void Delete(Membership membership)
        {
            this.memberships.Remove(membership);
        }

        public async Task<List<Membership>> FilterMembershipsAsync(bool? isActive, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;

            return await this.memberships
                .Include(s => s.Client)
                .Include(s => s.MembershipType)
                .Include(s => s.MembershipSessions)
                .WhereIf(isActive != null && isActive == true, s => now.Date >= s.From.Date && now.Date <= s.To.Date)
                .OrderByDescending(s => s.From.Date)
                .ToListAsync(cancellationToken);
        }

        public async Task<Membership?> GetMembershipByDateAsync(Guid clientId, DateTime startDate, CancellationToken cancellationToken)
        {
            return await this.memberships
                .Where(m => m.ClientId == clientId)
                .Where(m => startDate >= m.From && startDate <= m.To)
                .Include(s => s.Client)
                .Include(m => m.MembershipType)
                .Include(m => m.MembershipSessions)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Membership?> GetMembershipByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await this.memberships
                .Include(s => s.Client)
                .Include(m => m.MembershipType)
                .Include(m => m.MembershipSessions)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<Membership> GetMembershipByIdSafeAsync(Guid id, CancellationToken cancellationToken)
        {
            return await this.GetMembershipByIdAsync(id, cancellationToken) ?? throw new Exception("Not found.");
        }

        public async Task<List<Membership>> GetMembershipsByClientIdAsync(Guid clientId, CancellationToken cancellationToken)
        {
            return await this.memberships
                .Where(m => m.ClientId == clientId)
                .Include(s => s.MembershipType)
                .Include(m => m.MembershipSessions)
                .OrderByDescending(s => s.To)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Membership>> GetMembershipsByStartDateAsync(DateTime startDate, CancellationToken cancellationToken)
        {
            return await this.memberships
                .Where(m => m.From.Date <= startDate.Date && m.To.Date >= startDate.Date)
                .Include(s => s.Client)
                .Include(s => s.MembershipType)
                .Include(s => s.MembershipSessions)
                .OrderByDescending(s => s.From.Date)
                .ToListAsync(cancellationToken);
        }

        public void Update(Membership membership)
        {
            this.memberships.Update(membership);
        }
    }
}
