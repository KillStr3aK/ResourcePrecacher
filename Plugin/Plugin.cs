namespace ResourcePrecacher
{
    using CounterStrikeSharp.API.Core;
    using CounterStrikeSharp.API.Core.Attributes;

    using Microsoft.Extensions.Logging;

    [MinimumApiVersion(195)]
    public sealed partial class Plugin : BasePlugin, IPluginConfig<PluginConfig>
    {
        public required PluginConfig Config { get; set; } = new PluginConfig();

        private readonly PrecacheContext PrecacheContext;

        private readonly PluginMigrations Migrations;

        public Plugin(PluginMigrations migrations, PrecacheContext context)
        {
            this.Migrations = migrations;
            this.PrecacheContext = context;
        }

        public void OnConfigParsed(PluginConfig config)
        {
            if (config.Version < this.Config.Version)
            {
                Logger.LogWarning("Configuration is out of date. Consider updating the plugin.");

                if (this.Migrations.HasInstruction(config.Version, this.Config.Version))
                {
                    base.Logger.LogWarning("Instruction for migrating your config file: {0}", this.Migrations.GetInstruction(config.Version, this.Config.Version));
                } else
                {
                    base.Logger.LogWarning("No migrating instruction available");
                }
            }

            if (config.ResourceList.Count == 0)
            {
                base.Logger.LogWarning("'ResourceList' is empty, did you forget to populate the list with resources?");
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
        }
    }
}
