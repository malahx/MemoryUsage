#### MemoryUsage
#### A plugin for Kerbal Space Program 0.90
#### Copyright 2015 Malah

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>. 
 

#### What is it?

MemoryUsage is a plugin which adds the possibility to see the memory usage, the cpu usage and the count of threads used by KSP.

#### How to install it?

Unzip all files. Put the files which are in "Kerbal Space Program" folder in your game installation folder.

#### How to uninstall it?

Delete the MemoryUsage folder in your KSP/GameData folder and MemoryUsage.exe which is in your game installation folder.

#### How to use it?

To use this mod, you need to launch MemoryUsage.exe, if the game is not launched, MemoryUsage.exe will launch it. After this, in game, you just need to push on F11 to start seeing the KSP memory usage.
You can see the min/avg/max data if you hold F11 5 seconds.

If you want to launch the game in 64 bits, just put -64b in arguments: MemoryUsage.exe -64b

If you want to launch the game with opengl or anyother options just put it in arguments: MemoryUsage.exe -force-opengl

#### Why an external exe for this?

For security, Squad has blocked all the process functions used on KSP, with this, a modder can't make a malicious mod. 
But to know the amount of memory used by KSP we need to access to these functions.

If you don't want an external executable, you can try GCMonitor.

#### Troubleshooting?

I have tested only on linux 64b and windows 7 32b, I don't know if that will work on mac (I think it will not work, I need a tester).

If you are on linux, you will need to start MemoryUsage.exe with the option that you use to start KSP (ex: LC_ALL=C ...).

#### Changelog

v1.20 - 2015.02.19
- New: Added a min/avg/max function (with hold f11),
- New: Added a text color,
- New: Added an alarm (disabled by default, you can enable it on the file GameData/MemoryUsage/Config.txt, put Alarm to true),
- Fix: Delete the "Load" spam on the logs.

v1.11 - 2014.11.19
- Fix: Corrected the creation of memory.txt file at the startup of MemoryUsage.exe,
- Fix: Corrected the CPU usage which stop to check on Linux.

v1.10 - 2014.11.17
- New: Added the CPU usage and the threads count,
- New: Added a configfile to edit the Key to display the cpu/memory usage, you can find it at "Kerbal Space Program/GameData/MemoryUsage/PluginData/MemoryUsage/MemoryUsage.cfg",
- Fix: Corrected the KSP arguments,
- Minor fix.

v1.00 - 2014.11.07
- Initial release.

#### Thanks!

* to willy_ineedthatapp_com for his pup_alert sound,
* to all mods developers which make this game really huge,
* to my friend Neimad who corrects my bad english ...
* and to Squad for this awesome game.

#### Links

* http://forum.kerbalspaceprogram.com/threads/99494
* https://kerbalstuff.com/mod/317
* https://github.com/malahx/MemoryUsage
* pup_alert sound: http://www.freesound.org/people/willy_ineedthatapp_com/sounds/167337/
* GCMonitor: http://forum.kerbalspaceprogram.com/threads/92907