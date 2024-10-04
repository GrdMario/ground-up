namespace GroundUp.Api.Infrastructure.Database.Repositories
{
    using GroundUp.Api.Domain;
    using GroundUp.Api.Infrastructure.Database.Contracts;
    using GroundUp.Api.Infrastructure.Database.Internal;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class MembershipTypeRepository : IMembershipTypeRepository
    {
        private readonly DbSet<MembershipType> membershipTypes;

        public MembershipTypeRepository(GroundUpContext context)
        {
            this.membershipTypes = context.Set<MembershipType>();
        }

        public void Add(MembershipType entity)
        {
            this.membershipTypes.Add(entity);
        }

        public async Task<List<MembershipType>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await this.membershipTypes.ToListAsync(cancellationToken);
        }

        public async Task<MembershipType> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await this.membershipTypes.FindAsync(new object[] { id }, cancellationToken) ?? throw new Exception("Not found.");
        }

        public void Update(MembershipType entity)
        {
            this.membershipTypes.Update(entity);
        }
    }
}
