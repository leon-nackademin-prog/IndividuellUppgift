using IndividuellUppgiftUserStory.Entities;
using IndividuellUppgiftUserStory.Repositories;
using IndividuellUppgiftUserStory.Services;
using System.ComponentModel.Design;



EquipmentRepository repository = new EquipmentRepository();
EquipmentService service = new EquipmentService(repository);


EquipmentItem? selectedItem = ShowMenu();

EquipmentItem? ShowMenu()
{
    
    while (true)
    {
        Console.WriteLine("### Sök efter exemplar###");
        Console.WriteLine("### Ange inventarienummer:");

        string? input = Console.ReadLine()?.Trim();
        

        SearchResult result = service.SearchItem(input!);

        if (result.Success)
        {
            Console.WriteLine("Exemplaret hittades!");
            Console.WriteLine($"Inventarienummer: {result.Item!.InventoryNumber}");
            Console.WriteLine($"Modell: {result.Item.Model.Name}");
            Console.WriteLine($"Otillgänglig {result.Item.IsUnavailable}");

            Console.WriteLine("\nVill du välja exemplaret? J/N");

            string? choice = Console.ReadLine()?.Trim().ToUpper();

            if (choice == "J")
            {
                Console.WriteLine("Exemplaret är valt.");
                return result.Item;
            }
            else if (choice == "N")
            {
                if (!AskToRetry())
                {
                    Console.WriteLine("Sökningen avslutas.");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Ogiltigt val.");
            }
        }
        else
        {
            Console.WriteLine(result.Message);

            if (!AskToRetry())
            {
                Console.WriteLine("Sökningen avslutas.");
                return null;
            }
        }


       
       
    }

    bool AskToRetry()
    {
        while (true)
        {
            Console.WriteLine("Vill du söka igen? J/N");

            string? decision = Console.ReadLine()?.Trim().ToUpper();

            if (decision == "J")
            {
                return true;
            }
            else if (decision == "N")
            {
                return false;
            }
            else
            {
                Console.WriteLine("Du måste välja J eller N.");
            }
        }
    }
    
}
