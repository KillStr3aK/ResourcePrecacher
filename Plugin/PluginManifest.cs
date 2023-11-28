namespace ResourcePrecacher
{
    using CounterStrikeSharp.API.Core;

    public sealed partial class Plugin : BasePlugin
    {
        public override string ModuleName => "Resource Precacher";

        public override string ModuleAuthor => "Nexd @ Eternar (https://eternar.dev)";

        public override string ModuleDescription => "Automatically precache resources.";

        public override string ModuleVersion => "1.0.0 " +
#if RELEASE
            "(release)";
#else
            "(debug)";
#endif
    }
}
