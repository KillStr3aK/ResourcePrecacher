namespace ResourcePrecacher
{
    using CounterStrikeSharp.API.Core;
    using CounterStrikeSharp.API.Core.Plugin;

    using Microsoft.Extensions.Logging;

    public sealed class PrecacheContext
    {
        public required Plugin Plugin;

        private HashSet<string> Resources = new HashSet<string>();

        private readonly ILogger<PrecacheContext> Logger;

        private readonly PluginContext PluginContext;

        public int ResourceCount => this.Resources.Count;

        public PrecacheContext(ILogger<PrecacheContext> logger, IPluginContext pluginContext)
        {
            this.Logger = logger;
            this.PluginContext = (pluginContext as PluginContext)!;
        }

        public void Initialize()
        {
            this.Plugin = (this.PluginContext.Plugin as Plugin)!;

            this.Plugin.RegisterListener<Listeners.OnServerPrecacheResources>((manifest) =>
            {
                int precachedResources = 0;

                foreach (string resourcePath in this.Resources)
                {
                    if (this.Plugin.Config.Log)
                    {
                        this.Logger.LogInformation("Precaching \"{Resource}\" (context: {PrecacheContext}) [{Amount}/{Count}]", resourcePath, $"0x{manifest.Handle:X}", ++precachedResources, this.ResourceCount);
                    }

                    manifest.AddResource(resourcePath);
                }

                if (this.Plugin.Config.Log)
                {
                    this.Logger.LogInformation("Precached {ResourceCount} resources.", this.ResourceCount);
                }
            });
        }

        public bool AddResource(string resourcePath)
        {
            if (resourcePath.Contains('/'))
            {
                resourcePath = resourcePath.Replace('/', Path.DirectorySeparatorChar);
            }    

            return this.Resources.Add(resourcePath);
        }

        public bool RemoveResource(string resourcePath)
        {
            if (resourcePath.Contains('/'))
            {
                resourcePath = resourcePath.Replace('/', Path.DirectorySeparatorChar);
            }

            return this.Resources.Remove(resourcePath);
        }
    }
}
