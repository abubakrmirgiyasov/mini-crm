using Microsoft.EntityFrameworkCore;
using MiniCrm.UI.Models.DTO_s;
using MiniCrm.UI.Repositories.Interfaces;
using MiniCrm.UI.Services;

namespace MiniCrm.UI.Repositories;

public class ManagerRepository : IManagerRepository
{
    private readonly ApplicationDbContext _context;

    public ManagerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ManagerViewModel>> GetSampleManagersAsync()
    {
        try
        {
            var managers = await _context.Managers
                .Include(x => x.Employee)
                .ToListAsync();
            return ExtractingManagerDTO.ExtractingSampleViewModels(managers);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}
