using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ZamboniUltimateTeam.Packs
{
    public static class PackConfigSerializer
    {
        private static IDeserializer BuildDeserializer() =>
            new DeserializerBuilder()
                .WithNamingConvention(NullNamingConvention.Instance)
                .WithCaseInsensitivePropertyMatching()
                .IgnoreUnmatchedProperties()
                .Build();

        private static ISerializer BuildSerializer() =>
            new SerializerBuilder()
                .WithNamingConvention(PascalCaseNamingConvention.Instance)
                .ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitNull)
                .Build();

        public static PackConfig Deserialize(string yaml) =>
            BuildDeserializer().Deserialize<PackConfig>(yaml);

        public static PackConfig DeserializeFile(string path) =>
            Deserialize(File.ReadAllText(path));

        public static string Serialize(PackConfig config) =>
            BuildSerializer().Serialize(config);

        public static void SerializeToFile(PackConfig config, string path) =>
            File.WriteAllText(path, Serialize(config));
    }
}