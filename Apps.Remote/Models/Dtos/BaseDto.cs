using Newtonsoft.Json;

namespace Apps.Remote.Models.Dtos;

public class BaseDto<T>
{
    [JsonProperty("data")]
    public T? Data { get; set; }
}