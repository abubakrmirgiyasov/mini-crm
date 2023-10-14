namespace MiniCrm.UI.Models;

public class EmployeeProject
{
    public Guid? EmployeeId { get; set; }

    public Guid? ProjectId { get; set; }

    public Employee? Employee { get; set; } = null!;

    public Project? Project { get; set; } = null!;
}