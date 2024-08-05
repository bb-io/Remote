using Blackbird.Applications.Sdk.Common;

namespace Apps.Remote.Models.Responses.TimeOffs;

public class TimeOffResponse
{
    [Display("Employment ID")]
    public string EmploymentId { get; set; }

    [Display("End date")]
    public DateTime EndDate { get; set; }

    [Display("Time off ID")]
    public string Id { get; set; }

    [Display("Notes")]
    public string Notes { get; set; }

    [Display("Start date")]
    public DateTime StartDate { get; set; }

    [Display("Status")]
    public string Status { get; set; }

    [Display("Timeoff days")]
    public IEnumerable<TimeoffDay> TimeoffDays { get; set; }

    [Display("Time off type")]
    public string TimeoffType { get; set; }

    [Display("Timezone")]
    public string Timezone { get; set; }

    [Display("Display name")] public string DisplayName => $"{Status}; From {StartDate} till {EndDate}; {TimeoffType}";
}