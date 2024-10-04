namespace GroundUp.Api.Domain
{
    using System;

    public class MembershipType
    {
        public Guid Id { get; protected set; }

        public string Name { get; set; } = default!;

        public string Color { get; set; } = default!;

        protected MembershipType() { }

        public MembershipType(
            Guid id,
            string name,
            string color)
        {
            this.Id = id;
            this.Name = name;
            this.Color = color;
        }
    }
}
