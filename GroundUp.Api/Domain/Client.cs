namespace GroundUp.Api.Domain
{
    using System;
    using System.Collections.Generic;

    public class Client
    {
        public Guid Id { get; protected set; }

        public string Email { get; protected set; } = default!;

        public string PhoneNumber { get; protected set; } = default!;

        public string FirstName { get; protected set; } = default!;

        public string LastName { get; protected set; } = default!;

        public DateTime DateOfBirth { get; protected set; }

        public string Address { get; protected set; } = default!;

        public string City { get; protected set; } = default!;

        public string? Description { get; protected set; }

        public DateTime CreatedAt { get; protected set; }

        public List<Membership> Memberships { get; protected set; } = [];

        protected Client() { }

        public Client(
            string firstName,
            string lastName,
            string email,
            string phoneNumber,
            DateTime dateOfBirth,
            string address,
            string city,
            DateTime createdAt,
            string? desrciption)
        {
            this.Id = Guid.NewGuid();
            this.FirstName = firstName.Trim();
            this.LastName = lastName.Trim();
            this.Email = email.Trim();
            this.PhoneNumber = phoneNumber.Trim();
            this.DateOfBirth = dateOfBirth;
            this.Address = address.Trim();
            this.City = city.Trim();
            this.CreatedAt = createdAt;
            this.Description = desrciption;
        }

        public void UpdateInfo(
            string firstName,
            string lastName,
            string email,
            string phoneNumber,
            DateTime dateOfBirth,
            string address,
            string city,
            string? description)
        {
            this.FirstName = firstName.Trim();
            this.LastName = lastName.Trim();
            this.Email = email.Trim();
            this.PhoneNumber = phoneNumber.Trim();
            this.DateOfBirth = dateOfBirth;
            this.Address = address.Trim();
            this.City = city.Trim();
            this.Description = description;
        }
    }
}
