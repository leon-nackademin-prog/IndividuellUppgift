using IndividuellUppgiftUserStory.Entities;

namespace IndividuellUppgiftUserStory.Repositories;

public interface IEquipmentRepository
{
    EquipmentItem? GetItem(string inventoryNumber);
}
