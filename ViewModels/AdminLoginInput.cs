using System.ComponentModel.DataAnnotations;

namespace FlyJusticeLite.ViewModels;

public sealed class AdminLoginInput
{
    [Required, StringLength(80)]
    public string Username { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
