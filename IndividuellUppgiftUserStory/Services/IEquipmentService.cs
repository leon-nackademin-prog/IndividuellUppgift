using IndividuellUppgiftUserStory.Entities;

namespace IndividuellUppgiftUserStory.Services;

public interface IEquipmentService
{
    SearchResult SearchItem(string inventoryNumber);
}
