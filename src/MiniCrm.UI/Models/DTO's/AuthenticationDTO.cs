namespace MiniCrm.UI.Models.DTO_s;

public class AuthenticationResponse
{
    public string AccessToken { get; set; } = null!;
}

public class SignInBindingModel
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}
