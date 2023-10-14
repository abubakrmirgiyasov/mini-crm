#nullable disable

namespace MiniCrm.UI.Common;

public class AppSettings
{
    public ConnectionStrings ConnectionStrings { get; set; }
}

public class ConnectionStrings
{
    public string SqlServerConnection { get; set; }
}
