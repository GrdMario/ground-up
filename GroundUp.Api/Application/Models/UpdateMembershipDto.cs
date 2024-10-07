namespace GroundUp.Api.Application.Models
{
    using System;

    public class UpdateMembershipDto
    {
        public Guid Id { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public DateTime? FrozenDate { get; set; }

        public int SessionCount { get; set; }

        public Guid MembershipTypeId { get; set; }
    }
}
