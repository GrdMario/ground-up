namespace GroundUp.Api.Application.Models
{
    using GroundUp.Api.Domain;
    using System;

    public class MembershipSessionDto
    {
        public Guid Id { get; set; }

        public Guid MembershipId { get; set; }

        public bool IsCancelled { get; set; }

        public string? Comment { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public DateTime CreatedAt { get; set; }

        public static MembershipSessionDto FromMembershipSession(MembershipSession membershipSession)
        {
            var membershipSessionDto = new MembershipSessionDto
            {
                Id = membershipSession.Id,
                MembershipId = membershipSession.MembershipId,
                IsCancelled = membershipSession.IsCancelled,
                Start = membershipSession.Start,
                End = membershipSession.End,
                Comment = membershipSession.Comment,
                CreatedAt = membershipSession.CreatedAt
            };

            return membershipSessionDto;
        }
    }
}
