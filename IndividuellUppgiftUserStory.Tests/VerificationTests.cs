using IndividuellUppgiftUserStory.Entities;
using IndividuellUppgiftUserStory.Repositories;
using IndividuellUppgiftUserStory.Services;

namespace IndividuellUppgiftUserStory.Tests;

public class VerificationTests
{
    private readonly EquipmentRepository _repository;
    private readonly EquipmentService _service;

    public VerificationTests()
    {
        _repository = new EquipmentRepository();
        _service = new EquipmentService(_repository);
    }
 
    [Theory]
    [InlineData("123456", "Borrmaskin")]
    [InlineData("1110000", "Gaffeltruck")]
    public void SearchItem_ValidExistingNumber_ReturnsSuccessWithItem(string inventoryNumber, string expectedModelName)
    {
        SearchResult result = _service.SearchItem(inventoryNumber);

        Assert.True(result.Success);
        Assert.Null(result.Message);
        Assert.NotNull(result.Item);
        Assert.Equal(inventoryNumber, result.Item!.InventoryNumber);
        Assert.Equal(expectedModelName, result.Item.Model.Name);
    }

    [Theory]
    [InlineData("")]       
    [InlineData("   ")]    
    [InlineData("abc123")]   
    [InlineData("1234")] 
    [InlineData("12345678")] 
    public void SearchItem_InvalidFormat_ReturnsFailureWithMessage(string input)
    {
        SearchResult result = _service.SearchItem(input);

        Assert.False(result.Success);
        Assert.Equal("Ogiltigt inventarienummer", result.Message);
        Assert.Null(result.Item);
    }

    [Fact]
    public void SearchItem_ValidFormatButUnknownNumber_ReturnsNotFound()
    {
        SearchResult result = _service.SearchItem("654321");

        Assert.False(result.Success);
        Assert.Equal("Exemplaret hittades inte.", result.Message);
        Assert.Null(result.Item);
    }
    
    [Theory]
    [InlineData("12345")]
    [InlineData("1234567")]
    [InlineData("000000")]
    public void SearchItem_BoundaryLengthsAreValidFormat_ButNotFound(string inventoryNumber)
    {
        SearchResult result = _service.SearchItem(inventoryNumber);

        Assert.False(result.Success);
        Assert.Equal("Exemplaret hittades inte.", result.Message);
    }

    [Fact]
    public void Repository_GetItem_ExistingNumber_ReturnsItem()
    {
        EquipmentItem? item = _repository.GetItem("1110000");

        Assert.NotNull(item);
        Assert.Equal("Gaffeltruck", item!.Model.Name);
    }

    [Fact]
    public void Repository_GetItem_UnknownNumber_ReturnsNull()
    {
        Assert.Null(_repository.GetItem("654321"));
    }

    private class FakeRepository : IEquipmentRepository
    {
        public int GetItemCallCount { get; private set; }
        public string? LastInventoryNumber { get; private set; }
        public EquipmentItem? ItemToReturn { get; set; }

        public EquipmentItem? GetItem(string inventoryNumber)
        {
            GetItemCallCount++;
            LastInventoryNumber = inventoryNumber;
            return ItemToReturn;
        }
    }

    [Theory]
    [InlineData("")]
    [InlineData("abc")]
    [InlineData("1234")]
    [InlineData("12345678")]
    public void SearchItem_InvalidFormat_DoesNotCallRepository(string input)
    {
        var fake = new FakeRepository();
        var service = new EquipmentService(fake);

        service.SearchItem(input);

        Assert.Equal(0, fake.GetItemCallCount);
    }

    [Fact]
    public void SearchItem_ValidFormat_CallsRepositoryOnceWithSameNumber()
    {
        var fake = new FakeRepository();
        var service = new EquipmentService(fake);

        service.SearchItem("123456");

        Assert.Equal(1, fake.GetItemCallCount);
        Assert.Equal("123456", fake.LastInventoryNumber);
    }

    [Fact]
    public void SearchItem_RepositoryReturnsNull_ReturnsNotFound()
    {
        var fake = new FakeRepository { ItemToReturn = null };
        var service = new EquipmentService(fake);

        SearchResult result = service.SearchItem("123456");

        Assert.False(result.Success);
        Assert.Equal("Exemplaret hittades inte.", result.Message);
    }

    [Fact]
    public void SearchItem_RepositoryReturnsItem_ReturnsThatItem()
    {
        var model = new EquipmentModel("M01", "Testmodell", null, "Verktyg", 100m);
        var item = new EquipmentItem("123456", model);
        var fake = new FakeRepository { ItemToReturn = item };
        var service = new EquipmentService(fake);

        SearchResult result = service.SearchItem("123456");

        Assert.True(result.Success);
        Assert.Same(item, result.Item);
    }

    [Fact]
    public void SearchItem_TwoItemsOfSameModel_ReturnsTheCorrectOne()
    {
        SearchResult a = _service.SearchItem("123456");
        SearchResult b = _service.SearchItem("123457");

        Assert.Same(a.Item!.Model, b.Item!.Model);   
        Assert.NotSame(a.Item, b.Item);              
        Assert.Equal("123457", b.Item.InventoryNumber);
    }

    [Fact]
    public void SearchItem_DoesNotChangeItemStatusOrRepositoryContent()
    {
        EquipmentItem item = _repository.GetItem("123456")!;
        bool statusBefore = item.IsUnavailable;
        int countBefore = _repository.GetAllItems().Count;

        _service.SearchItem("123456");
        _service.SearchItem("654321");
        _service.SearchItem("abc");

        Assert.Equal(statusBefore, item.IsUnavailable);
        Assert.Equal(countBefore, _repository.GetAllItems().Count);
    }
}

