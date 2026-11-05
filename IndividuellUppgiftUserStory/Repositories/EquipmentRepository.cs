using IndividuellUppgiftUserStory.Entities;

namespace IndividuellUppgiftUserStory.Repositories;

public class EquipmentRepository
{
    private readonly List<EquipmentItem> _equipmentItems = new List<EquipmentItem>();
    //lägg till testobjekt i listan

    public EquipmentItem? GetItem(string inventoryNumber)
    {
        var items = GetAllItems();
        return items.FirstOrDefault(item => item.InventoryNumber == inventoryNumber);
    }

    public List<EquipmentItem> GetAllItems()
    {
        return _equipmentItems;
    }

    
}
