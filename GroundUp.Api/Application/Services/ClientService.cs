namespace GroundUp.Api.Application.Services
{
    using GroundUp.Api.Application.Models;
    using GroundUp.Api.Domain;
    using GroundUp.Api.Infrastructure.Database.Contracts;
    using GroundUp.Api.Services.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class ClientService : IClientService
    {
        private readonly IUnitOfWork uow;

        public ClientService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task CreateAsync(CreateClientDto clientDto, CancellationToken cancellationToken)
        {
            var client = new Client(
                clientDto.FirstName,
                clientDto.LastName,
                clientDto.Email,
                clientDto.PhoneNumber,
                clientDto.DateOfBirth,
                clientDto.Address,
                clientDto.City,
                clientDto.Description);

            this.uow.ClientRepository.Add(client);

            await this.uow.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var client = await this.uow.ClientRepository.GetByIdSafeAsync(id, cancellationToken);

            this.uow.ClientRepository.Delete(client);

            await this.uow.SaveChangesAsync(cancellationToken);
        }

        public async Task<ClientDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var client = await this.uow.ClientRepository.GetByIdSafeAsync(id, cancellationToken);

            return ClientDto.FromClient(client);
        }

        public async Task<List<ClientDto>> GetClientsAsync(
            string? firstName,
            string? lastName,
            int skip,
            int take,
            CancellationToken cancellationToken)
        {
            var clients = await this.uow.ClientRepository.GetAsync(firstName, lastName, skip, take, cancellationToken);

            return clients.Select(client => ClientDto.FromClient(client)).ToList();
        }

        public async Task UpdateAsync(UpdateClientDto clientDto, CancellationToken cancellationToken)
        {
            var client = await this.uow.ClientRepository.GetByIdSafeAsync(clientDto.Id, cancellationToken);

            client.UpdateInfo(
                clientDto.FirstName,
                clientDto.LastName,
                clientDto.Email,
                clientDto.PhoneNumber,
                clientDto.DateOfBirth,
                clientDto.Address,
                clientDto.City,
                clientDto.Description);

            this.uow.ClientRepository.Update(client);

            await this.uow.SaveChangesAsync(cancellationToken);
        }
    }
}
