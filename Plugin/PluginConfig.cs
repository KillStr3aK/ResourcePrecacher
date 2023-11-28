namespace ResourcePrecacher
{
    using CounterStrikeSharp.API.Core;

    using System.Text.Json.Serialization;

    public sealed class PluginConfig : BasePluginConfig
    {
        [JsonPropertyName("Resources")]
        public HashSet<string> ResourceList { get; set; } = new HashSet<string>();

        public WIN_LINUX<string> CreatePrecacheContextSignature { get; set; } = new(string.Empty, string.Empty);

        public WIN_LINUX<string> PrecacheResourceSignature { get; set; } = new(string.Empty, string.Empty);
    }
}
