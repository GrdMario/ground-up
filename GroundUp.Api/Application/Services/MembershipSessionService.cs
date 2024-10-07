namespace GroundUp.Api.Application.Services
{
    using GroundUp.Api.Application.Contracts;
    using GroundUp.Api.Application.Models;
    using GroundUp.Api.Domain;
    using GroundUp.Api.Infrastructure.Database.Contracts;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class MembershipSessionService : IMembershipSessionService
    {
        private readonly IUnitOfWork uow;

        public MembershipSessionService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task CreateAsync(CreateMembershipSessionDto dto, CancellationToken cancellationToken)
        {
            var membershipSession = new MembershipSession(
                dto.MembershipId,
                dto.Start,
                dto.End,
                dto.Comment);

            this.uow.MembershipSessionRepository.Create(membershipSession);

            await this.uow.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(UpdateMembershipSessionDto dto, CancellationToken cancellationToken)
        {
            var membershipSession = await this.uow.MembershipSessionRepository.GetByIdSafeAsync(dto.Id, cancellationToken);

            membershipSession.Update(
                dto.IsCancelled,
                dto.Start,
                dto.End,
                dto.Comment);

            this.uow.MembershipSessionRepository.Update(membershipSession);

            await this.uow.SaveChangesAsync(cancellationToken);
        }

        public async Task<MembershipSessionDto> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var membershipSession = await this.uow.MembershipSessionRepository.GetByIdSafeAsync(id, cancellationToken);

            return MembershipSessionDto.FromMembershipSession(membershipSession);
        }
    }
}
