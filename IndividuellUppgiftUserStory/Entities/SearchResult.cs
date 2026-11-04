namespace IndividuellUppgiftUserStory.Entities;

public class SearchResult
{
    public bool Success { get; set; }
    public string? Message { get; set; }

    public EquipmentItem Item { get; set; }

    public SearchResult(bool success, string? message, EquipmentItem item)
    {
        Success = success;
        Message = message;
        Item = item;
    }
}
