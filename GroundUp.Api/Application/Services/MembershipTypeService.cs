namespace GroundUp.Api.Application.Services
{
    using GroundUp.Api.Application.Contracts;
    using GroundUp.Api.Application.Models;
    using GroundUp.Api.Domain;
    using GroundUp.Api.Infrastructure.Database.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class MembershipTypeService : IMembershipTypeService
    {
        private readonly IUnitOfWork uow;

        public MembershipTypeService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task AddAsync(MembershipTypeDto entity, CancellationToken cancellationToken)
        {
            var membershipType = new MembershipType(entity.Id, entity.Name, entity.Color);

            this.uow.MembershipTypeRepository.Add(membershipType);

            await this.uow.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<MembershipTypeDto>> GetAllAsync(CancellationToken cancellationToken)
        {
            var types = await this.uow.MembershipTypeRepository.GetAllAsync(cancellationToken);

            return types.Select(t => MembershipTypeDto.FromMembershipType(t)).ToList();
        }

        public async Task<MembershipTypeDto> GetMembershipTypeByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var model = await this.uow.MembershipTypeRepository.GetByIdAsync(id, cancellationToken);

            return MembershipTypeDto.FromMembershipType(model);
        }

        public async Task UpdateAsync(MembershipTypeDto entity, CancellationToken cancellationToken)
        {
            var model = await this.uow.MembershipTypeRepository.GetByIdAsync(entity.Id, cancellationToken);

            model.Name = entity.Name;
            model.Color = entity.Color;

            this.uow.MembershipTypeRepository.Update(model);

            await this.uow.SaveChangesAsync(cancellationToken);
        }
    }
}
