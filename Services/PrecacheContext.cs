namespace ResourcePrecacher
{
    using CounterStrikeSharp.API.Core;
    using CounterStrikeSharp.API.Core.Plugin;

    using Microsoft.Extensions.Logging;

    using SteamDatabase.ValvePak;

    public sealed class PrecacheContext
    {
        public required Plugin Plugin;

        private HashSet<string> Resources = new HashSet<string>();

        private readonly ILogger<PrecacheContext> Logger;

        private readonly PluginContext PluginContext;

        public int ResourceCount => this.Resources.Count;

        public string AssetsDirectory => Path.Combine(this.Plugin.ModuleDirectory, "Assets");

        private HashSet<string> ResourceTypes = new HashSet<string>()
        {
            "vmdl",     "vmdl_c",
            "vpcf",     "vpcf_c",
            "vmat",     "vmat_c",
            "vcompmat", "vcompmat_c",
            "vtex",     "vtex_c",
            "vsnd",     "vsnd_c",
            "vdata",    "vdata_c",
            "vpost",    "vpost_c",
            "vsurf",    "vsurf_c",
            "vanmgrph", "vanmgrph_c",
            "vmix",     "vmix_c",
            "vnmclip",  "vnmclip_c",
            "vrman",    "vrman_c",
            "vrr_c",    "vrr_c",
            "vsc",
            "vsmart",   "vsmart_c",
            "vsnap",    "vsnap_c",
            "vsndevts", "vsndevts_c",
            "vsndgrps",
            "vsndstck", "vsndstck_c",
            "vsvg",     "vsvg_c",
            "vts",      "vts_c",
            "vxml",     "vxml_c"
        };

        public PrecacheContext(ILogger<PrecacheContext> logger, IPluginContext pluginContext)
        {
            this.Logger = logger;
            this.PluginContext = (pluginContext as PluginContext)!;
        }

        public void Initialize()
        {
            this.Plugin = (this.PluginContext.Plugin as Plugin)!;

            foreach (string vpkPath in Directory.EnumerateFiles(this.AssetsDirectory, "*.vpk", SearchOption.AllDirectories))
            {
                string packageName = Path.GetFileNameWithoutExtension(vpkPath);

                using (Package package = new Package())
                {
                    try
                    {
                        this.Logger.LogInformation("Reading Workshop Package: '{0}'", packageName);

                        package.Read(vpkPath);

                        foreach (KeyValuePair<string, List<PackageEntry>> fileType in package.Entries)
                        {
                            if (!this.ResourceTypes.Contains(fileType.Key))
                                continue;

                            foreach (PackageEntry entry in fileType.Value)
                            {
                                string fullPath = entry.GetFullPath();

                                if (!this.AddResource(fullPath))
                                {
                                    this.Logger.LogWarning("Duplicate entry for resource: '{0}'", fullPath);
                                }
                            }
                        }
                    } catch (Exception ex)
                    {
                        this.Logger.LogError("Unable to read package: '{0}' ({1})", packageName, ex.Message);
                    }
                }
            }

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

            string extension = Path.GetExtension(resourcePath)[1..];

            if (!this.ResourceTypes.Contains(extension))
            {
                this.Logger.LogError("Resource type '{0}' can not be precached. ({1})", extension, resourcePath);

                // it was handled "successfully", we only return false for duplicates because of HashSet<>
                return true;
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
