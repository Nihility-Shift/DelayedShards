[![](https://img.shields.io/badge/-Nihility_Shift-111111?style=just-the-label&logo=github&labelColor=24292f)](https://github.com/Nihility-Shift)
![](https://img.shields.io/badge/Game%20Version-[GameVersion]-111111?style=flat&labelColor=24292f&color=111111)
[![](https://img.shields.io/discord/1180651062550593536.svg?&logo=discord&logoColor=ffffff&style=flat&label=Discord&labelColor=24292f&color=111111)](https://discord.gg/g2u5wpbMGu "Void Crew Modding Discord")

# [UserModName]

Version [ModVersion]  
For Game Version [GameVersion]  
Developed by [Authors]  
Requires: [Dependencies]

---------------------

### 💡 Functions - **Stores functional shards when inserted into the astral map, which may be activated on demand**

- On insertion of a minefield or excort shard, a related counter is incrimented.
- Clients can request activation of stored data shards.
- Provides a GUI display of stored escort and minefield shards.
- Provides keybinds and commands for shard execution.
- Settings can be configured at any time via F5 menu.
- Data is propogated from host to clients with the mod.

### 🎮 Client Usage

- The Host must have the mod.
- The Pilot should have the mod.
- Shard count can be viewed in the bottom left corner when visible. Default visibility requires the local player to actively be piloting.
- Configure mod settings at F5 > Mod Settings > Delayed Shards.
- Activate Escort and Minefield shards with keys `2` and `3` while sitting in the pilot's seat.
- Clients without the mod can use `!ActivateShard` and `!CountShards` instead of keybinds.

### 👥 Multiplayer Functionality

- ✅ Client
  - The Pilot should have the mod
- ✅ Host
  - The host must have the mod
  - ***If the host leaves the game and the new host doesn't have the mod, all inserted shards will be lost!!!***
- ✅ Session
  - Requires the room to be marked as Mod_Session.

---------------------

## 🔧 Install Instructions - **Install following the normal BepInEx procedure.**

Ensure that you have [BepInEx 5](https://thunderstore.io/c/void-crew/p/BepInEx/BepInExPack/) (stable version 5 **MONO**) and [VoidManager](https://thunderstore.io/c/void-crew/p/NihilityShift/VoidManager/) installed.

#### ✔️ Mod installation - **Unzip the contents into the BepInEx plugin directory**

Drag and drop `[ModName].dll` into `Void Crew\BepInEx\plugins`


## Known Issues
  - If the host leaves the game and the new host doesn't have the mod, all inserted shards will be lost.
  - Audio for escort/minefield summon played on insertion rather than on execution.
