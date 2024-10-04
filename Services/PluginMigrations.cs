namespace ResourcePrecacher
{
    public class PluginMigrations : Dictionary<int, Dictionary<int, string>>
    {
        public PluginMigrations()
        {
            this.AddMigration(1, 2, "You have to update the 'Windows' key inside 'CreatePrecacheContextSignature', and change the 'Version' to 2. (The latest example configuration is always available at: https://github.com/KillStr3aK/ResourcePrecacher/blob/master/public/addons/counterstrikesharp/configs/plugins/ResourcePrecacher/ResourcePrecacher.json)");
            this.AddMigration(2, 3, "You have to add a 'Log' key from your configuration, and change the 'Version' to 3. (The latest example configuration is always available at: https://github.com/KillStr3aK/ResourcePrecacher/blob/master/public/addons/counterstrikesharp/configs/plugins/ResourcePrecacher/ResourcePrecacher.json)");
            this.AddMigration(3, 4, "[WINDOWS ONLY] Updated signatures, redownload the latest version. (The latest example configuration is always available at: https://github.com/KillStr3aK/ResourcePrecacher/blob/master/public/addons/counterstrikesharp/configs/plugins/ResourcePrecacher/ResourcePrecacher.json)");
            this.AddMigration(4, 5, "Changed functionality, remove signature related config values. (The latest example configuration is always available at: https://github.com/KillStr3aK/ResourcePrecacher/blob/master/public/addons/counterstrikesharp/configs/plugins/ResourcePrecacher/ResourcePrecacher.json)");
            this.AddMigration(5, 6, "You have to change the config 'Version' from 5 to 6. From version 6, you can upload your workshop vpks inside the 'Assets' folder and everything is precached automatically. (You can still use the 'ResourcesList' option in the config, but dont use both for the same or you get duplicate warnings)");
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
