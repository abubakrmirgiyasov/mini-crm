using MiniCrm.UI.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiniCrm.UI.Models;

public class Manager : Entity<Guid>
{
    public Guid? EmployeeId { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public Employee? Employee { get; set; } = null!;
}
