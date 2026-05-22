using ZamboniUltimateTeam.Packs;

namespace ZamboniUltimateTeam;

public static class UltimateTeam
{
    public static IServerProvider Server;
    public static PackConfig PackConfig { get; private set; }
    private static string _packConfigPath;
    
    public static void Initialize(string connectionString, IServerProvider provider, string packConfigPath)
    {
        UltimateDatabase.ConnectionString = connectionString;
        UltimateDatabase.CreateTables();
        Server = provider;
        
        _packConfigPath = packConfigPath;
        ReloadPackConfig();
    }
    
    public static void ReloadPackConfig()
    {
        if (!File.Exists(_packConfigPath))
        {
            PackConfig = new PackConfig();
            PackConfigSerializer.SerializeToFile(PackConfig, _packConfigPath);
            return;
        }
    
        PackConfig = PackConfigSerializer.DeserializeFile(_packConfigPath);
    }
}