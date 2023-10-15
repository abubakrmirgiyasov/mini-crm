using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniCrm.UI.Controllers;
using MiniCrm.UI.Models.DTO_s;
using MiniCrm.UI.Repositories.Interfaces;
using MiniCrm.UI.Test.Common;
using Moq;

namespace MiniCrm.UI.Test.Controllers;

public class HomeControllerTest
{
    [Fact]
    public async void Index_Returns_A_View_Result_With_A_List_Of_Projects()
    {
        // Arrange
        var mock = new Mock<IProjectRepository>();
        mock.Setup(x => x.GetProjectsAsync()).ReturnsAsync(DataList.GetProjects());
        var controller = new HomeController(mock.Object);

        // Act
        var res = await controller.Index("NameSortParam");
    
        // Assert
        var view = Assert.IsType<ViewResult>(res);
        var model = Assert.IsAssignableFrom<IEnumerable<ProjectViewModel>>(view.Model);
        Assert.Equal(DataList.GetProjects().Count(), model.Count());
    }

    [Fact]
    public void Privacy_Returns_A_View()
    {
        // Arrange
        var mock = new Mock<IProjectRepository>();
        var controller = new HomeController(mock.Object);

        // Act
        var res = controller.Privacy();

        // Assert
        var view = Assert.IsType<ViewResult>(res);
        Assert.Equal(null!, view.ViewName);
    }
}