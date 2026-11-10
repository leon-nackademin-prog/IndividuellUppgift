using IndividuellUppgiftUserStory.Entities;
using IndividuellUppgiftUserStory.Services;

namespace IndividuellUppgiftUserStory.Views;

public class SearchView
{
    private readonly IEquipmentService _service;
    private readonly TextReader _input;
    private readonly TextWriter _output;

    public SearchView(IEquipmentService service, TextReader input, TextWriter output)
    {
        _service = service;
        _input = input;
        _output = output;
    }

    public EquipmentItem? ShowMenu()
    {

        while (true)
        {
            _output.WriteLine("### Sök efter exemplar ###");
            _output.WriteLine("### Ange inventarienummer: ");

            string? input = _input.ReadLine()?.Trim();


            SearchResult result = _service.SearchItem(input!);

            if (result.Success)
            {
                _output.WriteLine("Exemplaret hittades!");
                _output.WriteLine($"Inventarienummer: {result.Item!.InventoryNumber}");
                _output.WriteLine($"Modell: {result.Item.Model.Name}");
                string status = result.Item.IsUnavailable ? "Otillgänglig" : "Tillgänglig";
                _output.WriteLine($"Status: {status}");

                _output.WriteLine("\nVill du välja exemplaret? J/N");

                string? choice = _input.ReadLine()?.Trim().ToUpper();

                if (choice == "J")
                {
                    _output.WriteLine("Exemplaret är valt.");
                    return result.Item;
                }
                else if (choice == "N")
                {
                    if (!AskToRetry())
                    {
                        _output.WriteLine("Sökningen avslutas.");
                        return null;
                    }
                }
                else
                {
                    _output.WriteLine("Ogiltigt val.");
                }
            }
            else
            {
                _output.WriteLine(result.Message);

                if (!AskToRetry())
                {
                    _output.WriteLine("Sökningen avslutas.");
                    return null;
                }
            }
        }
    }

    private bool AskToRetry()
    {
        while (true)
        {
            _output.WriteLine("Vill du söka igen? J/N");

            string? decision = _input.ReadLine()?.Trim().ToUpper();
            if (decision == null)
            {
                return false;
            }
            else if (decision == "J")
            {
                return true;
            }
            else if (decision == "N")
            {
                return false;
            }
            else
            {
                _output.WriteLine("Du måste välja J eller N.");
            }
        }
    }

}
