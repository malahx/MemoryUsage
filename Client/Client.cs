/* 
MemoryUsage
Copyright 2014 Malah

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
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Threading;
namespace MemoryUsage {
	public class Client {
		public static string VERSION = "1.00";

		private static bool isdebug = true;
		public static int is64 = 0;
		private static string directory = "";
		private static string file = "GameData/MemoryUsage/PluginData/MemoryUsage/memory.txt";
		private static Process exe;
		private static string KSP_Exe {
			get {
				return KSP_Exe_Plat[is64];
			}
		}
		public static string[] KSP_Exe_Plat {
			get {
				//string[] executable = { "linux32", "linux64", "mac32", "mac64", "win32", "win64" };
				string[] _executable = new string[] { "KSP.x86", "KSP.x86_64", "KSP.x86", "KSP.x86_64", "KSP", "KSP_x64" };
				string[] _value = new string[3];
				if (Environment.OSVersion.Platform == PlatformID.Unix) {
					_value [0] = _executable [0];
					_value [1] = _executable [1];
					_value [2] = "";
				} else if (Environment.OSVersion.Platform == PlatformID.MacOSX) {
					_value [0] = _executable [2];
					_value [1] = _executable [3];
					_value [2] = "";
				} else {
					_value [0] = _executable [4];
					_value [1] = _executable [5];
					_value [2] = ".exe";
				}
				return _value;
			}
		}
		private static void Save(long WorkingSet64, long VirtualMemorySize64) {
			string[] bench = new string[] {
				"WorkingSet64 = " + WorkingSet64,
				"VirtualMemorySize64 = " + VirtualMemorySize64,
			};
			if (System.IO.Directory.Exists (System.IO.Path.GetDirectoryName(directory + file))) {
				try {
					File.WriteAllLines (directory + file, bench);
				} catch {
					Thread.Sleep (1000);
				}
			} else {
				Debug ("Bad installation: " + directory + file);
			}
		}
		private static void Main(string[] args) {
			if (args.Contains ("-64b")) {
				is64 = 1;
			}
			if (System.IO.File.Exists (KSP_Exe + KSP_Exe_Plat[2]) && (Process.GetProcessesByName (KSP_Exe_Plat[0]).Length == 0 && Process.GetProcessesByName (KSP_Exe_Plat[1]).Length == 0)) {
				Debug ("Executing " + KSP_Exe + KSP_Exe_Plat[2]);
				exe = Process.Start (KSP_Exe + KSP_Exe_Plat[2], args.ToString());
				Save (-1, -1);
				Thread.Sleep (30000);
				exe = Process.GetProcessesByName (KSP_Exe) [0];
			} else {
				Debug ("Waiting ...");
				DateTime _now = DateTime.Now;
				DateTime _idle = _now;
				while (true) {
					if (Process.GetProcessesByName (KSP_Exe_Plat [0]).Length >= 1) {
						exe = Process.GetProcessesByName (KSP_Exe_Plat [0]) [0];
						break;
					}
					if (Process.GetProcessesByName (KSP_Exe_Plat [1]).Length >= 1) {
						exe = Process.GetProcessesByName (KSP_Exe_Plat [1]) [0];
						break;
					}
					if ((_now - _idle).TotalSeconds < 60) {
						Debug ("Idle exit");
						Environment.Exit(0);
					}
					_now = DateTime.Now;
					Thread.Sleep (900);
				}
			}
			Debug ("Start process information log.");
			while (!exe.HasExited) {
				directory = System.IO.Path.GetDirectoryName (exe.MainModule.FileName) + "/";
				exe.Refresh ();
				Save (exe.WorkingSet64, exe.VirtualMemorySize64);
				Thread.Sleep (9900);
			}
			exe.Close ();
			Debug ("Exit");
			Environment.Exit(0);
		}
		private static void Debug(string _string) {
			if (isdebug) {
				Console.WriteLine ("MemoryUsage(" + VERSION + "): " + _string);
			}
		}
	}
}