namespace IndividuellUppgiftUserStory.Entities;

public class EquipmentItem
{
    public string InventoryNumber { get; set; }
    public EquipmentModel Model { get; set; }
    public bool IsUnavailable { get; set; }
    public string UnavailabilityReason { get; private set; }


    public EquipmentItem(string inventoryNumber, EquipmentModel model, bool isUnavailable = false) 
        { 
            InventoryNumber = inventoryNumber;
            Model = model;
            IsUnavailable = isUnavailable;
        }


}
