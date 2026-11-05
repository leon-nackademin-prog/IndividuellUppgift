using IndividuellUppgiftUserStory.Entities;

namespace IndividuellUppgiftUserStory.Repositories;

public class EquipmentRepository : IEquipmentRepository
{
    private readonly List<EquipmentItem> _equipmentItems = new List<EquipmentItem>();

    public EquipmentRepository()
    {
        SeedTestData();
    }


    public EquipmentItem? GetItem(string inventoryNumber)
    {
        var items = GetAllItems();
        return items.FirstOrDefault(item => item.InventoryNumber == inventoryNumber);
    }

    public List<EquipmentItem> GetAllItems()
    {
        return _equipmentItems;
    }

    public void SeedTestData()
    {
        EquipmentModel model01 = new EquipmentModel("M01", "Borrmaskin", "Slagborrmaskin", "Verktyg", 150.00m);
        EquipmentItem item01 = new EquipmentItem("123456", model01);
        

        EquipmentModel model02 = new EquipmentModel("M02", "Gaffeltruck", "en truck för transport", "Fordon", 25000.00m);
        EquipmentItem item02 = new EquipmentItem("1110000", model02);

        _equipmentItems.Add(item01);
        _equipmentItems.Add(item02);

    }
    

}
