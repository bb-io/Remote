namespace Apps.Remote.Models.Dtos;

public class TimeOffBalanceRootDto
{
    public TimeOffBalanceDto TimeoffBalance { get; set; } = new();
}

public class TimeOffBalanceDto
{
    public double WorkingHoursPerDay { get; set; }

    public string ContractualLeaveEntitlementType { get; set; } = string.Empty;
    
    public double ContractualEntitled { get; set; }

    public List<LeaveEntitlementDto> TimeoffEntitlements { get; set; } = new();

    public double TotalEntitledDays { get; set; }

    public TakenDto Taken { get; set; } = new();
    
    public UpcomingDto UpcomingApproved { get; set; } = new();

    public UpcomingDto UpcomingRequested { get; set; } = new();
}

public class LeaveEntitlementDto
{
    public string Name { get; set; } = string.Empty;
    
    public string Type { get; set; } = string.Empty;

    public DateTime ExpiryDate { get; set; } 

    public bool CanExpire { get; set; }

    public RemainingDto Remaining { get; set; } = new();

    public TakenDto Taken { get; set; } = new();
    
    public EntitledDto Entitled { get; set; } = new();
}

public class EntitledDto
{
    public double Days { get; set; }
    
    public double Hours { get; set; }
}

public class RemainingDto
{
    public double Days { get; set; }
    
    public double Hours { get; set; }
}

public class TakenDto
{
    public double Days { get; set; }
    
    public double Hours { get; set; }
}

public class UpcomingDto
{
    public double Days { get; set; }

    public double Hours { get; set; }
}
