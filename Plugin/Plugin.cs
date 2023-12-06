namespace ResourcePrecacher
{
    using CounterStrikeSharp.API.Core;
    using CounterStrikeSharp.API.Core.Attributes;

    using Microsoft.Extensions.Logging;

    [MinimumApiVersion(84)]
    public sealed partial class Plugin : BasePlugin, IPluginConfig<PluginConfig>
    {
        public required PluginConfig Config { get; set; } = new PluginConfig();

        private readonly PrecacheContext PrecacheContext;

        public Plugin(PrecacheContext context)
        {
            this.PrecacheContext = context;
        }

        public void OnConfigParsed(PluginConfig config)
        {
            if (config.Version < this.Config.Version)
            {
                base.Logger.LogWarning("Configuration is out of date. Consider updating the plugin.");
            }

            if (string.IsNullOrEmpty(config.CreatePrecacheContextSignature.Get()))
            {
                throw new Exception("Signature is missing or invalid for 'CreatePrecacheContext'");
            }

            if (string.IsNullOrEmpty(config.PrecacheResourceSignature.Get()))
            {
                throw new Exception("Signature is missing or invalid for 'PrecacheResource'");
            }

            if (config.ResourceList.Count == 0)
            {
                base.Logger.LogWarning("ResourceList is empty, did you forget to populate the list with resources?");
            }

            this.Config = config;
        }

        public override void Load(bool hotReload)
        {
            if (hotReload)
            {
                Logger.LogWarning("Hotreloading {ModuleName} has no effect.", this.ModuleName);
            }

            this.PrecacheContext.Initialize();

            base.RegisterListener<Listeners.OnMapStart>(map =>
            {
                foreach (var resourcePath in this.Config.ResourceList)
                {
                    this.PrecacheContext.AddResource(resourcePath);
                }
            });
        }

        public override void Unload(bool hotReload)
        {
            this.PrecacheContext.Release();
        }
    }
}
