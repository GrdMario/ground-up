namespace GroundUp.Api.Models
{
    using System;

    public class MembershipTypeViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = default!;

        public string Color { get; set; } = default!;
    }
}
