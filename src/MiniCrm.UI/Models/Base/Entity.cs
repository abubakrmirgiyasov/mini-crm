#nullable disable

using System.ComponentModel.DataAnnotations;

namespace MiniCrm.UI.Models.Base;

public interface IHasKey<TKey>
{
    TKey Id { get; set; }
}

public class Entity<TKey> : IHasKey<TKey>
{
    [Key]
    public TKey Id { get; set; }

    public DateTimeOffset CreatedDate { get; } = DateTimeOffset.Now;

    public DateTimeOffset? UpdateDateTime { get; set; }
}
