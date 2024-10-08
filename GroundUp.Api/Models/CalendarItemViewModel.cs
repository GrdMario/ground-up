namespace GroundUp.Api.Models
{
    using System;

    public record CalendarItemViewModel(
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
