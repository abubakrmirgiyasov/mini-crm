using MiniCrm.UI.Models.DTO_s;

namespace MiniCrm.UI.Test.Common;
internal class DataList
{
    internal static IEnumerable<ProjectViewModel> GetProjects()
    {
        return new ProjectViewModel[]
        {
            new ProjectViewModel()
            {
                Id = Guid.NewGuid(),
                Name = "test1",
                CustomerCompany = "test1",
                PerformingCompany = "test1",
                Priority = 1,
            },
        };
    }
}
