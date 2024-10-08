﻿namespace ResourcePrecacher
{
    using CounterStrikeSharp.API.Core;
    using CounterStrikeSharp.API.Core.Attributes;

    using Microsoft.Extensions.Logging;

    [MinimumApiVersion(271)]
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
                this.Logger.LogWarning("Configuration is out of date. Consider updating the plugin.");

                if (this.Migrations.HasInstruction(config.Version, this.Config.Version))
                {
                    base.Logger.LogWarning("Instruction for migrating your config file: {0}", this.Migrations.GetInstruction(config.Version, this.Config.Version));
                } else
                {
                    base.Logger.LogWarning("No migrating instruction available");
                }
            }

            this.Config = config;
        }

        public override void Load(bool hotReload)
        {
            if (hotReload)
            {
                this.Logger.LogWarning("Hotreloading {ModuleName} has no effect.", this.ModuleName);
            }

            this.PrecacheContext.Initialize();

            if (this.Config.ResourceList.Count == 0)
            {
                base.Logger.LogWarning("'ResourceList' is empty, did you forget to upload the workshop packagre, or populate the list with resources?");
            }

            foreach (string resourcePath in this.Config.ResourceList)
            {
                if (!this.PrecacheContext.AddResource(resourcePath))
                {
                    this.Logger.LogWarning("Duplicate entry for resource: '{0}'", resourcePath);
                }
            }
        }
    }
}
