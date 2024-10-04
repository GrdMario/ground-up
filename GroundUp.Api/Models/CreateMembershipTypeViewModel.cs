namespace GroundUp.Api.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CreateMembershipTypeViewModel
    {
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; } = default!;

        [Required(AllowEmptyStrings = false)]
        public string Color { get; set; } = default!;
    }
}
