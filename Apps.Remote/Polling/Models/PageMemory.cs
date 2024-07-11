namespace Apps.Remote.Polling.Models;

public class PageMemory
{
    public List<PageMemoryDto> PageMemoryDtos { get; set; } = new();
    
    public DateTime? LastPollingTime { get; set; }
}