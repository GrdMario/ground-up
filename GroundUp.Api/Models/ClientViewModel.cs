namespace GroundUp.Api.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ClientViewModel
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string FirstName { get; set; } = default!;

        [Required(AllowEmptyStrings = false)]
        [MinLength(1)]
        public string LastName { get; set; } = default!;

        [Required(AllowEmptyStrings = false)]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required(AllowEmptyStrings = false)]
        public string PhoneNumber { get; set; } = default!;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string Address { get; set; } = default!;

        [Required(AllowEmptyStrings = false)]
        public string City { get; set; } = default!;

        public string? Description { get; set; } = default!;
    }
}
