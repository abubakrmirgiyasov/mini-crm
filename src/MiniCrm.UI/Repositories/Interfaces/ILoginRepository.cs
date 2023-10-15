using MiniCrm.UI.Models.DTO_s;

namespace MiniCrm.UI.Repositories.Interfaces;

public interface ILoginRepository
{
    Task<AuthenticationResponse> LoginAsync(SignInBindingModel model);
}
