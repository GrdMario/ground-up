namespace GroundUp.Api.Domain
{
    using System;

    public class MembershipSession
    {
        public Guid Id { get; protected set; }

        public Guid MembershipId { get; protected set; }

        public Membership Membership { get; protected set; } = default!;

        public string? Comment { get; protected set; }

        public bool IsCancelled { get; protected set; }

        public DateTime? Start { get; protected set; }

        public DateTime? End { get; protected set; }

        public DateTime CreatedAt { get; protected set; }

        protected MembershipSession() { }

        public MembershipSession(
            Guid membershipId,
            DateTime? start = null,
            DateTime? end = null,
            string? comment = null)
        {
            this.Id = Guid.NewGuid();
            this.MembershipId = membershipId;
            this.IsCancelled = false;
            this.Start = start;
            this.End = end;
            this.Comment = comment;
            this.CreatedAt = DateTime.UtcNow;
        }

        public void Update(
            bool isCancelled,
            DateTime? start,
            DateTime? end,
            string? comment)
        {
            this.IsCancelled = isCancelled;
            this.Start = start;
            this.End = end;
            this.Comment = comment;
        }
    }
}
