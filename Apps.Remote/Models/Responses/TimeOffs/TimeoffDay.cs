using Blackbird.Applications.Sdk.Common;

namespace Apps.Remote.Models.Responses.TimeOffs;

public class TimeoffDay
{
    [Display("Day")]
    public DateTime Day { get; set; }

    [Display("Hours")]
    public int Hours { get; set; }
}