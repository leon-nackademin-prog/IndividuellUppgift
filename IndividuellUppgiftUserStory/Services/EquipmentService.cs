using IndividuellUppgiftUserStory.Entities;
using IndividuellUppgiftUserStory.Repositories;

namespace IndividuellUppgiftUserStory.Services;

public class EquipmentService
{
    private readonly EquipmentRepository _repository;

    public EquipmentService(EquipmentRepository repository)
    {
        _repository = repository;
    }

    //ValidateSearch()

    public EquipmentItem? SearchItem(string inventoryNumber)
    {
        return _repository.GetItem(inventoryNumber);
    }
    
}
