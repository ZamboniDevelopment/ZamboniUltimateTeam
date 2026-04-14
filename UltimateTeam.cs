namespace ZamboniUltimateTeam;

public static class UltimateTeam
{
    public static IServerProvider Server;
    
    public static void Initialize(string connectionString, IServerProvider provider)
    {
        UltimateDatabase.ConnectionString = connectionString;
        UltimateDatabase.CreateTables();
        Server = provider;
    }
}