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
using System.Threading;

namespace MemoryUsage {
	internal class LogFile {
		private static string fileInfo = "GameData/" + MU.MOD + "/Memory.txt";
		internal static void Save(bool Init) {
			string[] _Usages;
			if (Init) {
				_Usages = new string[] {
					"VERSION = " + MU.VERSION,
					"CPUusage = " + -1,
					"Threads = " + -1,
					"WorkingSet64 = " + -1,
					"VirtualMemorySize64 = " + -1,
				};
			} else {
				_Usages = new string[] {
					"VERSION = " + MU.VERSION,
					"CPUusage = " + Usage.GetCPU,
					"Threads = " + Usage.GetThreads,
					"WorkingSet64 = " + Usage.GetPhysical,
					"VirtualMemorySize64 = " + Usage.GetVirtual,
				};
			}
			if (File.Exists (Exe.KSPApplicationRootPath + fileInfo)) {
				FileInfo _FileInfo = new FileInfo (Exe.KSPApplicationRootPath + fileInfo);
				while (_FileInfo.IsReadOnly) {
					MU.Log ("Can't write, waiting ..."); 
					Thread.Sleep (1100);
				}
			}
			if (Directory.Exists (Path.GetDirectoryName (Exe.KSPApplicationRootPath + fileInfo))) {
				try {
					File.WriteAllLines (Exe.KSPApplicationRootPath + fileInfo, _Usages);
				} catch {
					MU.Log ("Can't write, skipping ..."); 
					Thread.Sleep (500);
				}
			} else {
				MU.Log ("Bad installation: " + Exe.KSPApplicationRootPath + fileInfo);
			}
		}
	}
}
