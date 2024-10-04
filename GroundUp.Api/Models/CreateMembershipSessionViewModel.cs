namespace GroundUp.Api.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreateMembershipSessionViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please select client")]
        public Guid MembershipId { get; set; }

        public string? Comment { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}
