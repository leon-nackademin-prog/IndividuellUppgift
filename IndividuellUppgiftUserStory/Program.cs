using IndividuellUppgiftUserStory.Entities;
using IndividuellUppgiftUserStory.Repositories;
using IndividuellUppgiftUserStory.Services;
using IndividuellUppgiftUserStory.Views;



IEquipmentRepository repository = new EquipmentRepository();
IEquipmentService service = new EquipmentService(repository);
SearchView view = new SearchView(service, Console.In, Console.Out);

EquipmentItem? selectedItem = view.ShowMenu();

