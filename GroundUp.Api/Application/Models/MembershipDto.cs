namespace GroundUp.Api.Application.Models
{
    using GroundUp.Api.Domain;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MembershipDto
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public DateTime? PaidDate { get; set; }

        public int SessionCount { get; set; }

        public Guid MembershipTypeId { get; set; }

        public ClientDto Client { get; set; } = default!;

        public MembershipTypeDto MembershipType { get; set; } = new();

        public List<MembershipSessionDto> MembershipSessions { get; set; } = [];

        public static MembershipDto FromMembership(Membership membership)
        {
            var membershipDto = new MembershipDto
            {
                Id = membership.Id,
                ClientId = membership.ClientId,
                MembershipTypeId = membership.MembershipTypeId,
                From = membership.From,
                To = membership.To,
                PaidDate = membership.PaidDate,
                MembershipType = MembershipTypeDto.FromMembershipType(membership.MembershipType),
                MembershipSessions = membership.MembershipSessions.Select(session => MembershipSessionDto.FromMembershipSession(session)).ToList(),
                SessionCount = membership.SessionCount,
                Client = ClientDto.FromClient(membership.Client)
            };
            return membershipDto;
        }
    }
}
