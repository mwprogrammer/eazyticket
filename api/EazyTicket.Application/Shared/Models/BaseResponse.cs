namespace EazyTicket.Application.Shared.Models
{
    public class BaseResponse<T>
    {
        public string RequestId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = [];
        public T? Data { get; set; }
    }
}
