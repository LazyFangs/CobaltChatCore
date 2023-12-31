# CobaltChatCore
Twitch Chat Integration for Cobalt Core made by Lazy Fangs\
Compatible with modloader: [2.0.2](https://github.com/Ewanderer/CobaltCoreModLoader/releases/tag/v2.0.2)\
Compatible with game version: 1.0.6

## Current Features:
- Connecting to chat via IRC
- join, eject, clear, open, close, ban, unban commands
- Configuration file to set up your experience
- JOIN COMMAND - Selected chatter will be able to appear and talk as the enemy character or appear as a drone (vanilla drones only, Jupiter excluded)
- Chatters can control their own named drone! left, right, shoot - help or hinder the streamer!

## Setup
1. Download the latest release of CobaltChatCore and the indicated version of modloader
2. Load mod into modloader
3. **!!Press the warmup button!!**
4. Check the new CobaltChatCore tab
5. **Put in your channel name! The mod will not load/function without it**
6. **Press the Authorize button and follow the steps on the popup page. Without authorization, the mod can't communicate with twitch.** Authoriazation is granted for 60 days and can be revoked at any time from your twitch dashboard
7. You're ready to go! All changes you make in the tab wil save upon starting the game

## Current Commands
Each command is prepended with a command signal (^ by default, customizable in config tab)
| Command       | Who can use | What it do  |
| ------------- |---------------| ----- |
| join      | Everyone | Allows chatters to become enemies during encounters. Their profile picture will show up and they will be able to speak in-game. People are chosen based on who was chosen the least |
| eject      | Mods     | Removes the current chatter from the enemy spot and replaces with a dummy pilot. Also works on drones   |
| open | Mods      | Opens the queue for people to join. Default is open.    |
| close | Mods      | Closes  the queue, preventing new chatters from joining. Chatters currently in queue remain there.   |
| clear | Mods      | Clears queue, removing all chatters.   |
| ban | Mods      | Bans chatter from entering queue. Ejects them from the enemy spot if they are currently occupying it    |
| unban | Mods      | Unbans chatter, allowing them to join again    |
| left      | Everyone | Lets the named drone's owner move it left. Can only be done a number of times over a drone's lifespan, configured by streamer |
| right      | Everyone | Lets the named drone's owner move it right. Can only be done a number of times over a drone's lifespan, configured by streamer |
| shoot      | Everyone | Lets the named drone's owner shoot. Can only be done a number of times over a drone's lifespan, configured by streamer |

## Special thanks
Thanks to John and Ben for making Cobalt Core and being super helpfull to the modding community!\
Thanks to EWanderer for the awesome modloader!\
Thanks to InsaneDragon for the default chatter portait!\
Thanks to the rest of the Cobalt Core modding community for the support and help!
