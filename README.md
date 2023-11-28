# ResourcePrecacher

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

> [!WARNING]  
> Hotreloading this plugin won't have any effect. Precache context only occurs when a map starts.
