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
using System.Diagnostics;

namespace MemoryUsage {
	public class MU {

		internal readonly static string VERSION = "1.20";
		internal readonly static string MOD = "MemoryUsage";
		internal static bool isdebug = true;

		internal static void Log(string msg) {
			if (isdebug) {
				Console.WriteLine (MOD + "(" + VERSION + "): " + msg);
			}
		}


		internal static int is64 = 0;

		internal static string KSP_Exe {
			get {
				return KSP_Exe_Plat[is64];
			}
		}
		internal static string[] KSP_Exe_Plat {
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
		internal static bool IsKSPLaunched {
			get {
				return (Process.GetProcessesByName (KSP_Exe_Plat [0]).Length > 0 || Process.GetProcessesByName (KSP_Exe_Plat [1]).Length > 0);
			}
		}
		internal static bool IsLinux {
			get {
				return Environment.OSVersion.Platform == PlatformID.Unix;
			}
		}
		internal static bool IsWindows {
			get {
				return (Environment.OSVersion.Platform != PlatformID.Unix && Environment.OSVersion.Platform == PlatformID.MacOSX);
			}
		}
	}
}