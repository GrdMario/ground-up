namespace GroundUp.Api.Models
{
    using System;
    using System.Collections.Generic;

    public class MembershipViewModel
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public DateTime? FrozenDate { get; set; }

        public int SessionCount { get; set; }

        public MembershipTypeViewModel MembershipType { get; set; } = new();

        public List<MembershipSessionViewModel> MembershipSessions { get; set; } = new();

        public MembershipClientViewModel MembershipClient { get; set; } = new();
    }
}
