using IndividuellUppgiftUserStory.Repositories;
using IndividuellUppgiftUserStory.Services;



EquipmentRepository repository = new EquipmentRepository();
EquipmentService service = new EquipmentService(repository);
ShowMenu();

//SelectItem(item)

void ShowMenu()
{
    Console.WriteLine("### Sök efter exemplar###");
    Console.WriteLine("### Ange inventarienummer:");

    string? input = Console.ReadLine();
    
    if (!string.IsNullOrWhiteSpace(input))
    {
        var result = service.SearchItem(input);
    }

    else
    {
        Console.WriteLine("Du måste ange ett inventarienummer.");
    }
}
