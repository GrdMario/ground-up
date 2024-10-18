namespace GroundUp.Api.Services.Contracts
{
    using GroundUp.Api.Application.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IClientService
    {
        Task<List<ClientDto>> GetClientsAsync(
            string? firstName,
            string? lastName,
            int skip,
            int take, CancellationToken cancellationToken);

        Task UpdateAsync(UpdateClientDto clientDto, CancellationToken cancellationToken);

        Task CreateAsync(CreateClientDto clientDto, CancellationToken cancellationToken);

        Task DeleteAsync(Guid id, CancellationToken cancellationToken);

        Task<ClientDto> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        Task<List<ClientDto>> GetClientsByStartDateAndEndDateAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
    }
}
