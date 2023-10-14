using MiniCrm.UI.Models.Base;
using System.ComponentModel.DataAnnotations;

namespace MiniCrm.UI.Models;

public class Employee : Entity<Guid>
{
    [Required(ErrorMessage = "Заполните обязательное поле")]
    [MaxLength(255, ErrorMessage = "Максимальная длина строки 255 символов")]
    [MinLength(3, ErrorMessage = "Минимальная длина строки 3 символа")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "Заполните обязательное поле")]
    [MaxLength(255, ErrorMessage = "Максимальная длина строки 255 символов")]
    [MinLength(3, ErrorMessage = "Минимальная длина строки 3 символа")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "Заполните обязательное поле")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [MaxLength(255, ErrorMessage = "Максимальная длина строки 255 символов")]
    [MinLength(4, ErrorMessage = "Минимальная длина строки 4 символа")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public List<Manager> Managers { get; set; } = null!;

    public List<EmployeeProject> EmployeeProjects { get; set; } = null!;

    public List<Task> Authors { get; set; } = null!;

    public List<Task>? Executors { get; set; }
}
