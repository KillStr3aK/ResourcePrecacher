namespace ResourcePrecacher
{
    using CounterStrikeSharp.API;
    using CounterStrikeSharp.API.Core;
    using CounterStrikeSharp.API.Core.Plugin;
    using CounterStrikeSharp.API.Modules.Memory.DynamicFunctions;
    using CounterStrikeSharp.API.Modules.Utils;
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
         *     sub_1808D36B0("models/chicken/chicken.vmdl", a2); <- this function = PrecacheResource
         *     sub_1808D36B0("particles/critters/chicken/chicken_gone.vpcf", a2);
         *     return sub_1808D36B0("particles/critters/chicken/chicken_roasted.vpcf", a2);
         * }
         *
         * Based on information from 'Yarukon' (chicken demon)
         * Note: 'CreatePrecacheContext' and 'PrecacheResource' are not the real function names.
         */

        public required MemoryFunctionVoid<nint, nint, nint> CreatePrecacheContext { get; set; }

        public required Action<string, nint> PrecacheResource { get; set; }

        private HashSet<string> Resources = new HashSet<string>();

        private readonly ILogger<PrecacheContext> Logger;

        private readonly PluginContext PluginContext;

        public int ResourceCount => Resources.Count;

        public PrecacheContext(ILogger<PrecacheContext> logger, PluginContext pluginContext)
        {
            this.Logger = logger;
            this.PluginContext = pluginContext;
        }

        private HookResult InterceptPrecacheContext(DynamicHook hook)
        {
            nint precacheContext = hook.GetParam<nint>(1);
            int precachedResources = 0;

            foreach (var resourcePath in Resources)
            {
                this.Logger.LogInformation("Precaching \"{Resource}\" (context: {PrecacheContext}) [{Amount}/{Count}]", resourcePath, $"0x{precacheContext:X}", ++precachedResources, this.ResourceCount);
                PrecacheResource(resourcePath, precacheContext);
            }

            this.Logger.LogInformation("Precached {ResourceCount} resources.", this.ResourceCount);
            return HookResult.Continue;
        }

        private HookResult InterceptPrecacheContextPost(DynamicHook hook)
        {
            this.CreatePrecacheContext.Unhook(this.InterceptPrecacheContext, HookMode.Pre);
            this.CreatePrecacheContext.Unhook(this.InterceptPrecacheContextPost, HookMode.Post);
            return HookResult.Continue;
        }

        public void Initialize()
        {
            Plugin plugin = (this.PluginContext.Plugin as Plugin)!;

            this.CreatePrecacheContext = new(plugin.Config.CreatePrecacheContextSignature.Get());
            this.PrecacheResource = VirtualFunction.CreateVoid<string, nint>(plugin.Config.PrecacheResourceSignature.Get());

            this.CreatePrecacheContext.Hook(this.InterceptPrecacheContext, HookMode.Pre);
            this.CreatePrecacheContext.Hook(this.InterceptPrecacheContextPost, HookMode.Post);
        }

        public bool AddResource(string resourcePath)
        {
            if (resourcePath.Contains('/'))
            {
                resourcePath = resourcePath.Replace("/", "\\");
            }    

            return this.Resources.Add(resourcePath);
        }

        public bool RemoveResource(string resourcePath)
        {
            if (resourcePath.Contains('/'))
            {
                resourcePath = resourcePath.Replace("/", "\\");
            }

            return this.Resources.Remove(resourcePath);
        }

        public void EnsureContext()
        {
            Vector NULL_VECTOR = new Vector(IntPtr.Zero);
            QAngle NULL_ANGLE = new QAngle(IntPtr.Zero);

            // As we haven't even reached the precache context it should fail:
            // WARNING: RESOURCE_TYPE_MODEL resource 'models/chicken/chicken.vmdl' (7A10E5899D1BF356) requested but is not in the system. (Missing from from a manifest?)
            // TODO
            CChicken? chicken = Utilities.CreateEntityByName<CChicken>("chicken");

            if (chicken != null)
            {
                chicken.Teleport(NULL_VECTOR, NULL_ANGLE, NULL_VECTOR);
                chicken.DispatchSpawn();

                Server.NextFrame(() =>
                {
                    chicken.Remove();
                });
            }
        }
    }
}
