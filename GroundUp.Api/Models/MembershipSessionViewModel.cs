namespace GroundUp.Api.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class MembershipSessionViewModel
    {
        public Guid SessionId { get; set; }

        public Guid Id { get; set; }

        public Guid MembershipId { get; set; }

        public Guid ClientId { get; set; }

        [Required]
        public bool IsCancelled { get; set; }

        public string? Comment { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public DateTime CreatedAt { get; set; }

        public int Count { get; set; }
    }
}
