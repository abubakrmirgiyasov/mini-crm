using MiniCrm.UI.Models.DTO_s;

namespace MiniCrm.UI.Repositories.Interfaces;

public interface IManagerRepository
{
    Task<IEnumerable<ManagerViewModel>> GetSampleManagersAsync();
}
