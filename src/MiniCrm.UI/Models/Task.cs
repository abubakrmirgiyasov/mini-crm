using MiniCrm.UI.Common;
using MiniCrm.UI.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MiniCrm.UI.Models;

public class Task : Entity<Guid>
{
    [Required(ErrorMessage = "Заполните обязательное поле")]
    [MaxLength(255, ErrorMessage = "Максимальная длина строки 255 символов")]
    [MinLength(3, ErrorMessage = "Минимальная длина строки 3 символа")]
    public string Name { get; set; } = null!;

    [MaxLength(500, ErrorMessage = "Максимальная длина строки 255 символов")]
    [MinLength(3, ErrorMessage = "Минимальная длина строки 3 символа")]
    public string? Description { get; set; }

    public int Priority { get; set; } = 1;

    public Status Status { get; set; }

    public Guid AuthorId { get; set; }

    public Employee Author { get; set; } = null!;

    public Guid? ExecutorId { get; set; }

    public Employee? Executor { get; set; }

    public Guid ProjectId { get; set; }

    [ForeignKey(nameof(ProjectId))]
    public Project Project { get; set; } = null!;
}
