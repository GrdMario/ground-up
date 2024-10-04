namespace GroundUp.Api.Services.Contracts
{
    using GroundUp.Api.Application.Models;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface ICalendarService
    {
        Task<List<CalendarItemDto>> GetCalendarItemsAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken);
    }
}
