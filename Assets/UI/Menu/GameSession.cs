public static class GameSession
{
    public static bool IsEnthusiast { get; set; } = false;
    
    public static string UserEmail { get; set; } = "";
    public static string UserPIN { get; set; } = "";

    public static void ClearSession()
    {
        IsEnthusiast = false;
        UserEmail = "";
        UserPIN = "";
    }
}