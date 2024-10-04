namespace GroundUp.Api.Application.Models
{
    using System;

    public class CreateMembershipSessionDto
    {
        public Guid MembershipId { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public string? Comment { get; set; }
    }
}
