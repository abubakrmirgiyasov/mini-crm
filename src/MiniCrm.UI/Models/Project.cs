using MiniCrm.UI.Models.Base;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MiniCrm.UI.Models;

public class Project : Entity<Guid>
{
    [Required(ErrorMessage = "Заполните обязательное поле")]
    [MaxLength(255, ErrorMessage = "Максимальная длина строки 255 символов")]
    [MinLength(3, ErrorMessage = "Минимальная длина строки 3 символа")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Заполните обязательное поле")]
    [MaxLength(255, ErrorMessage = "Максимальная длина строки 255 символов")]
    [MinLength(3, ErrorMessage = "Минимальная длина строки 3 символа")]
    public string CustomerCompanyInfo { get; set; } = null!;

    [Required(ErrorMessage = "Заполните обязательное поле")]
    [MaxLength(255, ErrorMessage = "Максимальная длина строки 255 символов")]
    [MinLength(3, ErrorMessage = "Минимальная длина строки 3 символа")]
    public string PerformingCompanyInfo { get; set; } = null!;

    public DateTimeOffset? ExpirationDate { get; set; }

    public int Priority { get; set; } = 1;

    public Guid? ProjectManagerId { get; set; }

    [ForeignKey(nameof(ProjectManagerId))]
    public Manager? Manager { get; set; } = null!;

    public List<EmployeeProject> EmployeeProjects { get; set; } = null!;
}
