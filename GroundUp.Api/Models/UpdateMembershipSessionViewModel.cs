namespace GroundUp.Api.Models
{
    using System;

    public class UpdateMembershipSessionViewModel
    {
        public Guid Id { get; set; }

        public Guid MembershipId { get; set; }

        public bool IsCancelled { get; set; }

        public string? Comment { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }
    }
}
