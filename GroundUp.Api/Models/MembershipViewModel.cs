﻿namespace GroundUp.Api.Models
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

        public int SessionsLeft { get; set; }

        public int DaysLeft { get; set; }

        public bool IsFrozen { get; set; }

        public MembershipTypeViewModel MembershipType { get; set; } = new();

        public List<MembershipSessionViewModel> MembershipSessions { get; set; } = [];

        public MembershipClientViewModel MembershipClient { get; set; } = new();
    }
}
