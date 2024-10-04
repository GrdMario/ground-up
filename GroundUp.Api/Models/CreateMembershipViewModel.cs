namespace GroundUp.Api.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreateMembershipViewModel
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        [Required]
        public DateTime From { get; set; }

        [Required]
        public DateTime To { get; set; }

        public DateTime? PaidDate { get; set; }

        [Required]
        public int SessionCount { get; set; }

        [Required(AllowEmptyStrings =false, ErrorMessage = "Please select membership type.")]
        public Guid MembershipTypeId { get; set; }
    }
}
