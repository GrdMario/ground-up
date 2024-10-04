namespace GroundUp.Api.Application.Models
{
    using System;

    public class UpdateClientDto
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string? Description { get; set; }

        public UpdateClientDto(
            Guid id,
            string firstName,
            string lastName,
            string email,
            string phoneNumber,
            DateTime dateOfBirth,
            string address,
            string city,
            string? description)
        {
            this.Id = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
            this.PhoneNumber = phoneNumber;
            this.DateOfBirth = dateOfBirth;
            this.Address = address;
            this.City = city;
            this.Description = description;
        }
    }
}
