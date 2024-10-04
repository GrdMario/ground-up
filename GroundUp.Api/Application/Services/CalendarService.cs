namespace GroundUp.Api.Application.Services
{
    using GroundUp.Api.Application.Models;
    using GroundUp.Api.Infrastructure.Database.Contracts;
    using GroundUp.Api.Services.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    internal sealed class CalendarService : ICalendarService
    {
        private readonly IUnitOfWork uow;

        public CalendarService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<List<CalendarItemDto>> GetCalendarItemsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken)
        {
            var sessions = await this.uow.MembershipSessionRepository.GetByStartAndEndDateAsync(startDate, endDate, cancellationToken);
            
            var calendarItems = new List<CalendarItemDto>();

            foreach (var item in sessions)
            {
                if (item.Start != null && item.End != null)
                {
                    var calendarItem = new CalendarItemDto(
                        item.Id,
                        item.Start.Value,
                        item.End.Value,
                        item.Start.Value.Hour,
                        item.End.Value.Hour,
                        item.Membership.ClientId,
                        $"{item.Membership.Client.FirstName} {item.Membership.Client.LastName}",
                        item.Membership.MembershipType.Color,
                        item.Membership.MembershipType.Name,
                        item.IsCancelled
                    );

                    calendarItems.Add(calendarItem);
                }
            }

            return calendarItems;
        }
    }
}
