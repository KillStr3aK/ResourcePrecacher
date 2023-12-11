namespace ResourcePrecacher
{
    public class PluginMigrations : Dictionary<int, Dictionary<int, string>>
    {
        public PluginMigrations()
        {
            this.AddMigration(1, 2, "You have to update the 'Windows' key inside 'CreatePrecacheContextSignature', and change the 'Version' to 2. (The latest example configuration is always available at: https://github.com/KillStr3aK/ResourcePrecacher/blob/master/public/addons/counterstrikesharp/configs/plugins/ResourcePrecacher/ResourcePrecacher.json)");
            this.AddMigration(2, 3, "You have to add a 'Log' key from your configuration, and change the 'Version' to 3. (The latest example configuration is always available at: https://github.com/KillStr3aK/ResourcePrecacher/blob/master/public/addons/counterstrikesharp/configs/plugins/ResourcePrecacher/ResourcePrecacher.json)");
            this.AddMigration(1, 3, "Your config is very old. Grab the latest one from the plugin repository and dont forget to change the 'Version' to 3. (The latest example configuration is always available at: https://github.com/KillStr3aK/ResourcePrecacher/blob/master/public/addons/counterstrikesharp/configs/plugins/ResourcePrecacher/ResourcePrecacher.json)");
        }

        public bool HasInstruction(int fromVersion, int toVersion)
        {
            return base.ContainsKey(fromVersion) && base[fromVersion].ContainsKey(toVersion);
        }

        public string GetInstruction(int fromVersion, int toVersion)
        {
            return base[fromVersion][toVersion];
        }

        public void AddMigration(int fromVersion, int toVersion, string instruction)
        {
            base.Add(fromVersion, new()
            {
                [toVersion] = instruction
            });
        }
    }
}
