namespace GroundUp.Api.Application.Models
{
    using GroundUp.Api.Domain;
    using System;

    public class ClientDto
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = default!;

        public string PhoneNumber { get; set; } = default!;

        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public DateTime DateOfBirth { get; set; }

        public string Address { get; set; } = default!;

        public string City { get; set; } = default!;

        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public static ClientDto FromClient(Client client)
        {
            var clientDto = new ClientDto
            {
                Id = client.Id,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Address = client.Address,
                City = client.City,
                Description = client.Description,
                DateOfBirth = client.DateOfBirth,
                CreatedAt = client.CreatedAt,
            };

            return clientDto;
        }
    }
}
