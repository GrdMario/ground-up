namespace GroundUp.Api.Domain
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Membership
    {
        public Guid Id { get; protected set; }

        public Guid ClientId { get; protected set; }

        public Client Client { get; protected set; } = default!;

        public DateTime From { get; protected set; }

        public DateTime To { get; protected set; }

        public DateTime? FrozenDate { get; protected set; }

        public int SessionCount { get; protected set; }

        public Guid MembershipTypeId { get; protected set; }

        public MembershipType MembershipType { get; protected set; } = default!;

        public List<MembershipSession> MembershipSessions { get; protected set; } = [];

        protected Membership() { }

        public Membership(
            Guid clientId,
            DateTime from,
            DateTime to,
            int sessionsCount,
            Guid membershipTypeId)
        {
            this.Id = Guid.NewGuid();
            this.ClientId = clientId;
            this.From = from;
            this.To = to;

            this.MembershipTypeId = membershipTypeId;
            this.SessionCount = sessionsCount;

            this.MembershipSessions =
                Enumerable.Range(
                    0,
                    this.SessionCount)
                .Select(s => new MembershipSession(this.Id))
                .ToList();
        }


        public void Update(
            DateTime from,
            DateTime to,
            Guid membershipTypeId,
            DateTime? fronzenDate)
        {
            this.From = from;
            this.To = to;
            this.MembershipTypeId = membershipTypeId;
            this.FrozenDate = fronzenDate;
        }
    }
}
