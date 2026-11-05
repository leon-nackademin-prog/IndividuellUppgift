using IndividuellUppgiftUserStory.Entities;
using IndividuellUppgiftUserStory.Repositories;
using System.Text.RegularExpressions;

namespace IndividuellUppgiftUserStory.Services;

public partial class EquipmentService
{
    private readonly EquipmentRepository _repository;

    [GeneratedRegex(@"\A[0-9]{5,7}\z")]
    private static partial Regex InventoryNumberPattern();

    public EquipmentService(EquipmentRepository repository)
    {
        _repository = repository;
    }


    public SearchResult SearchItem(string inventoryNumber)
    {
        if (!ValidateSearch(inventoryNumber))
        {
            return new SearchResult(false, "Ogiltigt inventarienummer", null);
        }

        EquipmentItem? item = _repository.GetItem(inventoryNumber);

        if (item == null)
        {
            return new SearchResult(false, "Exemplaret hittades inte.", null);
        }

        return new SearchResult(true, null, item);
    }

    private bool ValidateSearch(string? inventoryNumber)
    {
        if (string.IsNullOrWhiteSpace(inventoryNumber))
        {
            return false;
        }

        if (InventoryNumberPattern().IsMatch(inventoryNumber))
        {
            return true;
        }
        else
        {
            return false;
        }
         
    }
}
    
