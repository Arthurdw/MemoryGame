namespace Memory_Game
{
    internal static class SqlStatements
    {
        public static string SaveUser = "INSERT INTO scores (score, username, game) " +
                                        "VALUES (@score, @username, @game);";

        public static string GetLatestUser = "SELECT game " +
                                             "FROM scores " +
                                             "ORDER BY saved_at DESC " +
                                             "LIMIT 1;";
    }
}