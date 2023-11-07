using DomainLayer.Enums;

namespace ApplicationLayer.DTOs
{
    public class ActionResultData
    {
        public ActionResultData()
        {
            Status = Status.Failed;
        }

        public Status Status { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public string StatusString => Status.ToString();
    }
}
