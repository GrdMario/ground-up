namespace GroundUp.Api.Application.Models
{
    using GroundUp.Api.Domain;
    using System;

    public class MembershipSessionDto
    {
        public Guid Id { get; set; }

        public Guid MembershipId { get; set; }

        public bool IsCancelled { get; set; }

        public bool IsCompleted { get; set; }

        public string? Comment { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public DateTime CreatedAt { get; set; }

        public static MembershipSessionDto FromMembershipSession(MembershipSession membershipSession)
        {
            var membershipSessionDto = new MembershipSessionDto();

            membershipSessionDto.Id = membershipSession.Id;
            membershipSessionDto.MembershipId = membershipSession.MembershipId;
            membershipSessionDto.IsCompleted = membershipSession.IsCompleted;
            membershipSessionDto.IsCancelled = membershipSession.IsCancelled;
            membershipSessionDto.Start = membershipSession.Start;
            membershipSessionDto.End = membershipSession.End;
            membershipSessionDto.Comment = membershipSession.Comment;
            membershipSessionDto.CreatedAt = membershipSession.CreatedAt;

            return membershipSessionDto;
        }
    }
}
