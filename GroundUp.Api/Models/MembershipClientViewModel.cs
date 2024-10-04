namespace GroundUp.Api.Models
{
    using System;

    public class MembershipClientViewModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;
    }
}
