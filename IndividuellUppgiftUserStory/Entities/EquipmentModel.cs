namespace IndividuellUppgiftUserStory.Entities;

public class EquipmentModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string Category { get; set; }
    public decimal DailyRate { get; set; }

    public EquipmentModel(string id, string name, string? description, string category, decimal dailyRate)
    {
        Id = id;
        Name = name;
        Description = description;
        Category = category;
        DailyRate = dailyRate;
    }
}
