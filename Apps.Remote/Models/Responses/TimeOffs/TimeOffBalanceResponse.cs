using Apps.Remote.Models.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Remote.Models.Responses.TimeOffs;

public class TimeOffBalanceResponse
{
    [Display("Contractual entitled")]
    public double ContractualEntitled { get; set; }

    [Display("Timeoff entitlements")]
    public List<LeaveEntitlementResponse> TimeoffEntitlements { get; set; } = new();

    [Display("Taken days")]
    public double TakenDays { get; set; }

    [Display("Taken hours")]
    public double TakenHours { get; set; }
    
    [Display("Upcoming approved days")]
    public double UpcomingApprovedDays { get; set; }
    
    [Display("Upcoming approved hours")]
    public double UpcomingApprovedHours { get; set; }
    
    [Display("Upcoming requested days")]
    public double UpcomingRequestedDays { get; set; }
    
    [Display("Upcoming requested hours")]
    public double UpcomingRequestedHours { get; set; }

    [Display("Total entitled days")]
    public double TotalEntitledDays { get; set; }

    [Display("Working hours per day")]
    public double WorkingHoursPerDay { get; set; }

    public TimeOffBalanceResponse(TimeOffBalanceDto dto)
    {
        ContractualEntitled = dto.ContractualEntitled;
        TakenDays = dto.Taken.Days;
        TakenHours = dto.Taken.Hours;
        UpcomingApprovedDays = dto.UpcomingApproved.Days;
        UpcomingApprovedHours = dto.UpcomingApproved.Hours;
        UpcomingRequestedDays = dto.UpcomingRequested.Days;
        UpcomingRequestedHours = dto.UpcomingRequested.Hours;
        TotalEntitledDays = dto.TotalEntitledDays;
        WorkingHoursPerDay = dto.WorkingHoursPerDay;
        foreach (var leaveEntitlement in dto.TimeoffEntitlements)
        {
            TimeoffEntitlements.Add(new LeaveEntitlementResponse
            {
                EntitledDays = leaveEntitlement.Entitled.Days,
                EntitledHours = leaveEntitlement.Entitled.Hours,
                ExpiryDate = leaveEntitlement.ExpiryDate,
                Name = leaveEntitlement.Name,
                RamainingDays = leaveEntitlement.Remaining.Days,
                RamainingHours = leaveEntitlement.Remaining.Hours,
                TakenDays = leaveEntitlement.Taken.Days,
                TakenHours = leaveEntitlement.Taken.Hours,
                LeaveType = leaveEntitlement.Type,
                CanExpire = leaveEntitlement.CanExpire
            });
        }
    }
}

public class LeaveEntitlementResponse
{
    public string Name { get; set; } = string.Empty;
    
    [Display("Leave type")]
    public string LeaveType { get; set; } = string.Empty;
    
    [Display("Expiry date")]
    public DateTime ExpiryDate { get; set; }

    [Display("Can expire")]
    public bool CanExpire { get; set; }
    
    [Display("Entitled days")]
    public double EntitledDays { get; set; }
    
    [Display("Entitled hours")]
    public double EntitledHours { get; set; }

    [Display("Remaining days")]
    public double RamainingDays { get; set; }
    
    [Display("Remaining hours")]
    public double RamainingHours { get; set; }
    
    [Display("Taken days")]
    public double TakenDays { get; set; }
    
    [Display("Taken hours")]
    public double TakenHours { get; set; }
}