> [!IMPORTANT]  
> Now supports loading resources from VPK files.

# ResourcePrecacher
This plugin can precache custom resources.

# What can it do
You can upload or add any number of resources either in the `Assets` folder or in the configuration file and it will automatically precache them when the map starts.

These files are supported:

```json
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
```

# What cannot it do
It cannot download the resources for the clients.

# Configuration

Upload your workshop packages (the ones that you have uploaded to workshop) into the "Assets" folder inside the plugin folder. (`plugins/ResourcePrecacher/Assets`)
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

> [!WARNING]  
> Hotreloading this plugin has no effect because the resources can only be precached on map start. If you hotreload the plugin you are supposed to change the map atleast to take effect.
