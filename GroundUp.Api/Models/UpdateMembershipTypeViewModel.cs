namespace GroundUp.Api.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UpdateMembershipTypeViewModel
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = default!;

        [Required(AllowEmptyStrings = false)]
        public string Color { get; set; } = default!;
    }
}
