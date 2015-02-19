/* 
MemoryUsage
Copyright 2015 Malah

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
*/

using System;
using System.IO;
using UnityEngine;

namespace MemoryUsage {
	internal class LogFile {

		private static string SaveFile = "GameData/" + MU.MOD + "/Memory.txt";

		internal static void Load() {
			if (File.Exists (SaveFile)) {
				ConfigNode _temp = ConfigNode.Load (SaveFile);
				Check.ClientVERSION = _temp.GetValue ("VERSION");
				Check.CPU.Current = int.Parse (_temp.GetValue ("CPUusage"));
				Check.Threads.Current = int.Parse (_temp.GetValue ("Threads"));
				Check.Physical.Current = Int64.Parse (_temp.GetValue ("WorkingSet64"));
				Check.Virtual = Int64.Parse (_temp.GetValue ("VirtualMemorySize64"));
				Check.lastCheck = File.GetLastWriteTime (SaveFile);
				Check.bug = 0;
			} else {
				Check.bug = 1;
			}
		}
	}
}