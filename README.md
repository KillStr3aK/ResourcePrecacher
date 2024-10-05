> [!IMPORTANT]  
> Now supports loading resources from VPK files.

# ResourcePrecacher
This plugin can precache custom resources.

# What can it do
You can add any number of resources in the configuration and it will automatically precache them when the map starts.

Any resource type can be added to the configuration.

> [!WARNING]  
> Hotreloading this plugin has no effect because the resources can only be precached on map start. If you hotreload the plugin you are supposed to change the map atleast to take effect.

# What cannot it do
It cannot download the resources for the clients.

# Configuration

Upload your assets into the "Assets" folder inside the plugin folder. (`plugins/ResourcePrecacher/Assets`)
You don't have to list these, each of them will be checked so you can upload as many as possible.

Folder Structure:

plugins
 - ResourcePrecacher
    - Assets
        - 123456789.vpk
        - 987654321.vpk
        - assets.vpk
        - models.vpk

**OR**

Add your resources in the configuration:

```jsonc
{
  "Resources": [
     "models/props_office/file_cabinet_03.vmdl",
     "models/props_exteriors/guardrail512.vmdl",
     // ..
     // ..
     // ..
  ]
}
```

You can use both ways even together, just dont include resources that is already in an uploaded vpk.
