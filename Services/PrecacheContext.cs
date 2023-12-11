namespace ResourcePrecacher
{
    using CounterStrikeSharp.API.Core;
    using CounterStrikeSharp.API.Core.Plugin;
    using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
    using CounterStrikeSharp.API.Modules.Memory;

    using Microsoft.Extensions.Logging;

    public sealed class PrecacheContext
    {
        /*
         * string: models/chicken/chicken.vmdl
         * 
         * __int64 __fastcall sub_180228730(__int64 a1, __int64 a2) <- this function = CreatePrecacheContext
         * {
         *    if ( (unsigned __int8)sub_18057A680() )
         *       return sub_180688390(a1, a2);
         *     sub_1806BE3C0(a1, a2);
         *     sub_1808D36B0("models/chicken/chicken.vmdl", a2); <- this function = PrecacheResource and 'a2' is the precache context
         *     sub_1808D36B0("particles/critters/chicken/chicken_gone.vpcf", a2);
         *     return sub_1808D36B0("particles/critters/chicken/chicken_roasted.vpcf", a2);
         * }
         *
         * Based on information from 'Yarukon' (chicken demon)
         * Note: 'CreatePrecacheContext' and 'PrecacheResource' are not the real function names.
         */

        public required MemoryFunctionVoid<nint, nint, nint> CreatePrecacheContext { get; set; }

        public required Action<string, nint> PrecacheResource { get; set; }

        public required Plugin Plugin;

        private HashSet<string> Resources = new HashSet<string>();

        private readonly ILogger<PrecacheContext> Logger;

        private readonly PluginContext PluginContext;

        public int ResourceCount => Resources.Count;

        public PrecacheContext(ILogger<PrecacheContext> logger, IPluginContext pluginContext)
        {
            this.Logger = logger;
            this.PluginContext = (pluginContext as PluginContext)!;
        }

        public void Initialize()
        {
            this.Plugin = (this.PluginContext.Plugin as Plugin)!;

            this.CreatePrecacheContext = new(this.Plugin.Config.CreatePrecacheContextSignature.Get());
            this.PrecacheResource = VirtualFunction.CreateVoid<string, nint>(this.Plugin.Config.PrecacheResourceSignature.Get());

            this.CreatePrecacheContext.Hook(this.InterceptPrecacheContext, HookMode.Pre);
        }

        private HookResult InterceptPrecacheContext(DynamicHook hook)
        {
            nint precacheContext = hook.GetParam<nint>(1);
            int precachedResources = 0;

            foreach (string resourcePath in this.Resources)
            {
                if (this.Plugin.Config.Log)
                {
                    this.Logger.LogInformation("Precaching \"{Resource}\" (context: {PrecacheContext}) [{Amount}/{Count}]", resourcePath, $"0x{precacheContext:X}", ++precachedResources, this.ResourceCount);
                }

                PrecacheResource(resourcePath, precacheContext);
            }

            if (this.Plugin.Config.Log)
            {
                this.Logger.LogInformation("Precached {ResourceCount} resources.", this.ResourceCount);
            }

            return HookResult.Continue;
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

        public void Release()
        {
            this.CreatePrecacheContext.Unhook(this.InterceptPrecacheContext, HookMode.Pre);
        }
    }
}
