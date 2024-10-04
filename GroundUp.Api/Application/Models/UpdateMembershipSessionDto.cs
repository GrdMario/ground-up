namespace GroundUp.Api.Application.Models
{
    using System;

    public class UpdateMembershipSessionDto
    {
        public Guid Id { get; set; }

        public Guid MembershipId { get; set; }

        public bool IsCancelled { get; set; }

        public bool IsCompleted { get; set; }

        public string? Comment { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }
    }
}
