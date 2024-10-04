namespace GroundUp.Api.Infrastructure.Database.Internal
{
    using GroundUp.Api.Infrastructure.Database.Contracts;
    using System.Threading;
    using System.Threading.Tasks;

    public class UnitOfWork : IUnitOfWork
    {
        private readonly GroundUpContext context;

        public UnitOfWork(
            GroundUpContext context,
            IClientRepository clientRepository,
            IMembershipRepository membershipRepository,
            IMembershipSessionRepository membershipSessionRepository,
            IMembershipTypeRepository membershipTypeRepository)
        {
            this.context = context;
            this.ClientRepository = clientRepository;
            this.MembershipRepository = membershipRepository;
            this.MembershipSessionRepository = membershipSessionRepository;
            this.MembershipTypeRepository = membershipTypeRepository;
        }

        public IClientRepository ClientRepository { get; }

        public IMembershipRepository MembershipRepository { get; }

        public IMembershipSessionRepository MembershipSessionRepository { get; }

        public IMembershipTypeRepository MembershipTypeRepository { get; }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await this.context.SaveChangesAsync(cancellationToken);
        }
    }
}
