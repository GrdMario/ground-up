namespace GroundUp.Api.Infrastructure.Database.Contracts
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IUnitOfWork
    {
        public IClientRepository ClientRepository { get; }

        public IMembershipRepository MembershipRepository { get; }

        public IMembershipSessionRepository MembershipSessionRepository { get; }

        public IMembershipTypeRepository MembershipTypeRepository { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
