namespace IndividuellUppgiftUserStory.Entities;

public class SearchResult
{
    public bool Success { get; }
    public string? Message { get; }

    public EquipmentItem? Item { get; }

    public SearchResult(bool success, string? message, EquipmentItem? item) 
    {
        Success = success;
        Message = message;
        Item = item;
    }
}
