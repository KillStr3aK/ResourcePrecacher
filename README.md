# ResourcePrecacher

# What can it do
You can add any number of resources in the configuration and it will automatically precache them when the map starts.

> [!WARNING]  
> Hotreloading this plugin has no effect because the resources can only be precached on map start. If you hotreload the plugin you are supposed to change the map atleast to take effect.

# What cannot it do
It cannot download the resources for the clients.

# Configuration

Add your resources in the configuration:

```jsonc
{
  "Resources": [
     "models/props_office/file_cabinet_03.vmdl",
     "models/props_exteriors/guardrail512.vmdl",
     // ..
     // ..
     // ..
  ],
  "CreatePrecacheContextSignature": {
    // ...
  },
  "PrecacheResourceSignature": {
    // ...
  },
  "ConfigVersion": 1
}
```
