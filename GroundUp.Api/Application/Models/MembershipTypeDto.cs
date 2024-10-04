namespace GroundUp.Api.Application.Models
{
    using GroundUp.Api.Domain;
    using System;

    public class MembershipTypeDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string Color { get; set; } = default!;

        public static MembershipTypeDto FromMembershipType(MembershipType membershipType)
        {
            var membershipTypeDto = new MembershipTypeDto();

            membershipTypeDto.Id = membershipType.Id;
            membershipTypeDto.Name = membershipType.Name;
            membershipTypeDto.Color = membershipType.Color;

            return membershipTypeDto;
        }
    }
}
