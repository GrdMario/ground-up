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

    internal sealed class ClientRepository : IClientRepository
    {
        private readonly DbSet<Client> clients;

        public ClientRepository(GroundUpContext context)
        {
            this.clients = context.Set<Client>();
        }

        public async Task<List<Client>> GetAsync(string? firstName, string? lastName, int skip, int take, CancellationToken cancellationToken)
        {
            return await this.clients
                .WhereIf(firstName != null, s => s.FirstName == firstName)
                .WhereIf(lastName != null, s => s.LastName == lastName)
                .Skip(skip)
                .Take(take)
                .OrderBy(ob => ob.FirstName)
                .ToListAsync(cancellationToken);
        }

        public async Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await this.clients.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<Client> GetByIdSafeAsync(Guid id, CancellationToken cancellationToken)
        {
            return await this.GetByIdAsync(id, cancellationToken) ?? throw new Exception("not found.");
        }

        public void Add(Client client)
        {
            this.clients.Add(client);
        }

        public void Update(Client client)
        {
            this.clients.Update(client);
        }

        public void Delete(Client client)
        {
            this.clients.Remove(client);
        }

        public async Task<List<Client>> GetByStartDateAndEndDateAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            return await this.clients
                .Where(c => c.CreatedAt >= startDate && c.CreatedAt <= endDate)
                .ToListAsync(cancellationToken);
        }
    }
}
