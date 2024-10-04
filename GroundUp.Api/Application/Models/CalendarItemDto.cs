namespace GroundUp.Api.Application.Models
{
    using System;

    public record CalendarItemDto(
        Guid MembershipSessionId,
        DateTime MembershipSessionStartDate,
        DateTime MembershipSessionEndDate,
        int StartTime,
        int EndTime,
        Guid ClientId,
        string ClientName,
        string MembershipTypeColor,
        string MembershipTypeName,
        bool IsCancelled);
}
