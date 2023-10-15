namespace MiniCrm.UI.Common;

public class Constants
{
    public static string[] ROLES = new string[] { "manager", "project_manager", "employee" };
}

public enum Status : int
{
    Todo = 1,
    InProgress = 2,
    Done = 3,
}
