using IndividuellUppgiftUserStory.Entities;
using IndividuellUppgiftUserStory.Repositories;
using IndividuellUppgiftUserStory.Services;
using IndividuellUppgiftUserStory.Views;

namespace IndividuellUppgiftUserStory.Tests;

public class SearchViewTests
{
    private readonly EquipmentRepository _repository = new EquipmentRepository();

    private (EquipmentItem? Result, string Output) RunView(string input, IEquipmentService? service = null)
    {
        var output = new StringWriter();
        var view = new SearchView(
            service ?? new EquipmentService(_repository),
            new StringReader(input),
            output);

        EquipmentItem? result = view.ShowMenu();

        return (result, output.ToString());
    }

    [Fact]
    public void ShowMenu_FoundAndConfirmed_ReturnsSelectedItem()
    {
        var (result, output) = RunView("123456\nJ\n");

        Assert.Same(_repository.GetItem("123456"), result);
        Assert.Contains("Exemplaret hittades!", output);
        Assert.Contains("Exemplaret är valt.", output);
    }

    [Fact]
    public void ShowMenu_FoundItem_ShowsInventoryNumberAndModel()
    {
        var (_, output) = RunView("123456\nJ\n");

        Assert.Contains("Inventarienummer: 123456", output);
        Assert.Contains("Borrmaskin", output);
    }

    [Fact]
    public void ShowMenu_FoundUnavailableItem_ShowsStatus()
    {
        var model = new EquipmentModel("M99", "Lift", null, "Fordon", 500m);
        _repository.GetAllItems().Add(new EquipmentItem("999999", model, isUnavailable: true));

        var (_, output) = RunView("999999\nJ\n");
        
        Assert.Contains("Status: Otillgänglig", output);
    }

    [Fact]
    public void ShowMenu_InvalidFormatThenNo_ShowsErrorAndEndsSearch()
    {
        var (result, output) = RunView("abc\nN\n");

        Assert.Null(result);
        Assert.Contains("Ogiltigt inventarienummer", output);
        Assert.Contains("Vill du söka igen? J/N", output);
        Assert.Contains("Sökningen avslutas.", output);
    }

    [Fact]
    public void ShowMenu_InvalidFormatThenYes_SearchesAgainAndSucceeds()
    {
        var (result, _) = RunView("abc\nJ\n123456\nJ\n");

        Assert.Equal("123456", result!.InventoryNumber);
    }

    [Fact]
    public void ShowMenu_NotFoundThenNo_ShowsErrorAndEndsSearch()
    {
        var (result, output) = RunView("654321\nN\n");

        Assert.Null(result);
        Assert.Contains("Exemplaret hittades inte.", output);
        Assert.Contains("Sökningen avslutas.", output);
    }

    [Fact]
    public void ShowMenu_NotFoundThenYes_SearchesAgainAndSucceeds()
    {
        var (result, _) = RunView("654321\nJ\n1110000\nJ\n");

        Assert.Equal("1110000", result!.InventoryNumber);
    }


    [Fact]
    public void ShowMenu_DeclineSelectionThenNo_EndsSearch()
    {
        var (result, output) = RunView("123456\nN\nN\n");

        Assert.Null(result);
        Assert.DoesNotContain("Exemplaret är valt.", output);
        Assert.Contains("Sökningen avslutas.", output);
    }

    [Fact]
    public void ShowMenu_DeclineSelectionThenYes_CanSelectAnotherItem()
    {
        var (result, _) = RunView("123456\nN\nJ\n1110000\nJ\n");

        Assert.Equal("1110000", result!.InventoryNumber);
    }

    [Fact]
    public void ShowMenu_InvalidChoiceOnSelectQuestion_ShowsMessageAndRestartsSearch()
    {
        var (result, output) = RunView("123456\nX\n123456\nJ\n");

        Assert.Contains("Ogiltigt val.", output);
        Assert.Equal("123456", result!.InventoryNumber);
    }

    [Fact]
    public void ShowMenu_InvalidAnswerOnRetryQuestion_RepeatsQuestion()
    {
        var (result, output) = RunView("654321\nX\nN\n");

        Assert.Null(result);
        Assert.Contains("Du måste välja J eller N.", output);
    }

    [Fact]
    public void ShowMenu_InputEndsImmediately_EndsWithoutHanging()
    {
        var (result, _) = RunView("");

        Assert.Null(result);
    }

    [Fact]
    public void ShowMenu_InputEndsDuringRetryQuestion_EndsWithoutHanging()
    {
        var (result, _) = RunView("654321\n");

        Assert.Null(result);
    }

    private class FakeService : IEquipmentService
    {
        public List<string> ReceivedInputs { get; } = new();
        public SearchResult ResultToReturn { get; set; } = new SearchResult(false, "Fake-fel", null);

        public SearchResult SearchItem(string inventoryNumber)
        {
            ReceivedInputs.Add(inventoryNumber);
            return ResultToReturn;
        }
    }

    [Fact]
    public void ShowMenu_CallsSearchItemOncePerSearch_WithTrimmedInput()
    {
        var fake = new FakeService();

        RunView("  123456  \nJ\n999\nN\n", fake);

        Assert.Equal(new[] { "123456", "999" }, fake.ReceivedInputs);
    }

    [Fact]
    public void ShowMenu_ShowsMessageFromService()
    {
        var fake = new FakeService
        {
            ResultToReturn = new SearchResult(false, "Meddelande från servicen", null)
        };

        var (_, output) = RunView("123456\nN\n", fake);

        Assert.Contains("Meddelande från servicen", output);
    }

    [Fact]
    public void ShowMenu_SelectingItem_DoesNotChangeStatus()
    {
        var (result, _) = RunView("123456\nJ\n");

        Assert.False(result!.IsUnavailable);
        Assert.Equal(3, _repository.GetAllItems().Count);
    }

    [Fact]
    public void ShowMenu_FoundAvailableItem_ShowsAvailableStatus()
    {
        var (_, output) = RunView("123456\nJ\n");

        Assert.Contains("Status: Tillgänglig", output);
    }
}
