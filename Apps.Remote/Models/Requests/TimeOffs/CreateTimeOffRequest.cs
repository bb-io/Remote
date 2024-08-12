namespace Apps.Remote.Models.Requests.TimeOffs;

public class CreateTimeOffRequest
{
    public string EmploymentId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public string TimeoffType { get; set; }

    public string Timezone { get; set; }

    public DateTime ApprovedAt { get; set; }

    public string ApproverId { get; set; }

    public string Status { get; set; }
    
    public IEnumerable<TimeoffDayRequest> TimeoffDays { get; set; }

    public CreateTimeOffRequest(CreateTimeOffInput input)
    {
        EmploymentId = input.EmploymentId;
        StartDate = input.StartDate;
        EndDate = input.EndDate;
        TimeoffType = input.TimeoffType;
        Timezone = input.Timezone;
        ApprovedAt = input.ApprovedAt;
        ApproverId = input.ApproverId;
        Status = "approved";

        if (StartDate.DayOfYear == EndDate.DayOfYear)
        {
            TimeoffDays =
            [
                new()
                {
                    Day = StartDate.Date.ToString("yyyy-MM-dd"),
                    Hours = EndDate.Hour - StartDate.Hour
                }
            ];
            return;
        }

        var timeoffDays = new List<TimeoffDayRequest>();
        for (var i = 0; i <= EndDate.DayOfYear - StartDate.DayOfYear; i++)
        {
            timeoffDays.Add(new()
            {
                Hours = 8,
                Day = StartDate.AddDays(i).ToString("yyyy-MM-dd")
            });
        }

        TimeoffDays = timeoffDays;
    }
}