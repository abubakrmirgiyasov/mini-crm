using MiniCrm.UI.Models.Base;

namespace MiniCrm.UI.Models;

public class Role : Entity<Guid>
{
    public string Name { get; set; } = null!;

    public string NormalizedName { get; set; } = null!;

    public List<EmployeeRole> EmployeeRoles { get; set; } = null!;
}
