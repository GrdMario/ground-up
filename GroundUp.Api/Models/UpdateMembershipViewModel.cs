namespace GroundUp.Api.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UpdateMembershipViewModel
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime From { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime To { get; set; }

        public DateTime? FrozenDate { get; set; }

        [Required]
        public int SessionCount { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select membership type.")]
        public Guid MembershipTypeId { get; set; }
    }
}
