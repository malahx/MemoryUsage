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

// usage:	MemoryUsage.exe [-64b] [ksp_args]

// example:	MemoryUsage.exe -64b -force-opengl
//			It will start KSP in 64 bits with opengl


using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace MemoryUsage {
	public class Exe : MU {

		internal static Process KSP;
		internal static string KSPApplicationRootPath = "";
		private static string fileConfig = "GameData/" + MOD + "/Config.txt";
		private static string[] argsList = { "-64b", "-nostart", "-forcestart" };

		private static void StartKSP(string[] args) {
			Log ("Executing " + KSP_Exe + KSP_Exe_Plat[2]);
			KSP = Process.Start (KSP_Exe + KSP_Exe_Plat [2], string.Join (" ", args.Except(argsList)));
			Thread.Sleep (30000);
			KSP = Process.GetProcessesByName (KSP_Exe) [0];
		}

		private static void WaitKSP() {
			Log ("Waiting KSP Started ...");
			DateTime _now = DateTime.Now;
			DateTime _idle = _now;
			while (true) {
				if (Process.GetProcessesByName (KSP_Exe_Plat [0]).Length >= 1) {
					KSP = Process.GetProcessesByName (KSP_Exe_Plat [0]) [0];
					break;
				}
				if (Process.GetProcessesByName (KSP_Exe_Plat [1]).Length >= 1) {
					KSP = Process.GetProcessesByName (KSP_Exe_Plat [1]) [0];
					break;
				}
				if ((_now - _idle).TotalSeconds > 60) {
					Log ("Idle exit.");
					Environment.Exit (0);
				}
				_now = DateTime.Now;
				Thread.Sleep (1000);
			}
		}

		private static void Main(string[] args) {
			if (args.Contains (argsList [0])) {
				is64 = 1;
			}
			if (!args.Contains (argsList[1]) && File.Exists (KSP_Exe + KSP_Exe_Plat[2]) && !IsKSPLaunched || args.Contains (argsList[2])) {
				StartKSP (args);
			} else {
				WaitKSP ();
			}
			KSPApplicationRootPath = Path.GetDirectoryName (KSP.MainModule.FileName) + "/";
			if (!File.Exists (KSPApplicationRootPath + fileConfig)) {
				Log ("MemoryUsage plugin is not installed, MemoryUsage.exe shutdown.");
				Environment.Exit(0);
			}
			Log ("Scan Started ...");
			LogFile.Save (true);
			Thread.Sleep (1000);
			while (!KSP.HasExited) {
				KSP.Refresh ();
				LogFile.Save (false);
				Thread.Sleep (1000);
			}
			KSP.Close ();
			Log ("Exit");
			Environment.Exit(0);
		}
	}
}