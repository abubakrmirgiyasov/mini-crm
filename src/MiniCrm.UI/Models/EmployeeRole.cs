namespace MiniCrm.UI.Models;

public class EmployeeRole
{
    public Guid RoleId { get; set; }

    public Role Role { get; set; } = null!;

    public Guid EmployeeId { get; set; }

    public Employee Employee { get; set; } = null!;
}
