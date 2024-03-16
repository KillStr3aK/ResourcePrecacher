namespace ResourcePrecacher
{
    using CounterStrikeSharp.API.Core;

    using System.Text.Json.Serialization;

    public sealed class PluginConfig : BasePluginConfig
    {
        [JsonPropertyName("Resources")]
        public HashSet<string> ResourceList { get; set; } = new HashSet<string>();

        public bool Log { get; set; } = true;

        public override int Version { get; set; } = 5;
    }
}
