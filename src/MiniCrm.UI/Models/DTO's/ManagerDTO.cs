namespace MiniCrm.UI.Models.DTO_s;

public record ManagerViewModel(
    Guid? Value = null,
    string? Label = null);

public class ExtractingManagerDTO
{
    public static IEnumerable<ManagerViewModel> ExtractingSampleViewModels(List<Manager> models)
    {
        var managers = new List<ManagerViewModel>();

        foreach (var model in models)
        {
            managers.Add(new ManagerViewModel()
            {
                Value = model.Employee?.Id,
                Label = $"{model.Employee?.FirstName} {model.Employee?.LastName}",
            });
        }

        return managers;
    }
}
