namespace GroundUp.Api.Infrastructure.Database.Contracts
{
    using GroundUp.Api.Domain;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IClientRepository
    {
        Task<List<Client>> GetAsync(string? firstName, string? lastName, int skip, int take, CancellationToken cancellationToken);

        Task<Client> GetByIdSafeAsync(Guid id, CancellationToken cancellationToken);

        Task<Client?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        void Add(Client client);

        void Update(Client client);

        void Delete(Client client);
    }
}
