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
		public static string VERSION = "1.11";

		private static bool isdebug = true;
		public static int is64 = 0;
		//private static int CPUusage = 0;
		private static DateTime lastDate = DateTime.Now;
		private static double lastProcessorTime = 0;
		private static string directory = "";
		private static string file = "GameData/MemoryUsage/PluginData/MemoryUsage/memory.txt";
		private static string[] argsList = { "-64b" };
		// Don't know why, but on Arch linux 64b, TotalProcessorTime always return 0 with mono 3.10
		private static string[] LinuxCPUusageCLI = { "ps", "h -C {0} -o pcpu" };
		private static Process exe;
		//private static Thread ThGetCPUusage;
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
		private static bool IsKSPLaunched {
			get {
				return (Process.GetProcessesByName (KSP_Exe_Plat [0]).Length > 0 || Process.GetProcessesByName (KSP_Exe_Plat [1]).Length > 0);
			}
		}
		private static bool IsLinux {
			get {
				return Environment.OSVersion.Platform == PlatformID.Unix;
			}
		}
		private static bool IsWindows {
			get {
				return (Environment.OSVersion.Platform != PlatformID.Unix && Environment.OSVersion.Platform == PlatformID.MacOSX);
			}
		}
		private static int GetCPUusage {
			get {
				try {
					DateTime _currentDate = DateTime.Now;
					double _currentPorcessorTime = exe.TotalProcessorTime.TotalMilliseconds;
					int _CPUusage = (int)(100 * (_currentPorcessorTime - lastProcessorTime) / (_currentDate - lastDate).TotalMilliseconds / Environment.ProcessorCount);
					// Don't know why, but on Arch linux 64b, TotalProcessorTime always return 0 with mono 3.10
					if (IsLinux) {
						if (_CPUusage == 0) {
							PerformanceCounter CPUcounter = new PerformanceCounter();
							CPUcounter.CategoryName = "Process";
							CPUcounter.CounterName = "% Processor Time";
							CPUcounter.InstanceName = exe.ProcessName;
							_CPUusage = (int)CPUcounter.NextValue();
						}
						if (_CPUusage == 0) {
							ProcessStartInfo _startInfo = new ProcessStartInfo ();
							Process _exe = new Process ();
							_startInfo.FileName = LinuxCPUusageCLI[0];
							_startInfo.Arguments = string.Format (LinuxCPUusageCLI [1], exe.ProcessName);
							_startInfo.RedirectStandardOutput = true;
							_startInfo.UseShellExecute = false;
							_startInfo.CreateNoWindow = true;
							_exe.StartInfo = _startInfo;
							_exe.Start();
							string _output = _exe.StandardOutput.ReadToEnd ();
							if (!_exe.HasExited) {
								_exe.Kill();
							}
							_exe.Close();
							_CPUusage = (int)(double.Parse(_output) / Environment.ProcessorCount);
							Debug("CPU usage with CLI(" + _startInfo.FileName + " " + _startInfo.Arguments + "): " + _CPUusage + "%");
						} else {
							Debug("CPU usage with PerformanceCounter: " + _CPUusage + "%");
						}
					}
					lastProcessorTime = _currentPorcessorTime;
					lastDate = _currentDate;
					return _CPUusage;
				} catch (Exception e) {
					Debug ("ERROR: " + e.Message);
				}
				return 0;
			}
		} 
		// It seem to doesn't work on windows and return 0 on linux with mono 3.10
		/*private static void GetCPUusage() {
			try {
				PerformanceCounter CPUcounter = new PerformanceCounter();
				CPUcounter.CategoryName = "Process";
				CPUcounter.CounterName = "% Processor Time";
				CPUcounter.InstanceName = exe.ProcessName;
				while (!exe.HasExited) {
					CPUusage = (int)CPUcounter.NextValue();
					Thread.Sleep (1000);
				}
			}
			finally {
				Debug ("Thread ended");
			}
		}*/
		private static void Save(int CPUusage, int Threads, long WorkingSet64, long VirtualMemorySize64) {
			string[] bench = new string[] {
				"CPUusage = " + CPUusage,
				"Threads = " + Threads,
				"WorkingSet64 = " + WorkingSet64,
				"VirtualMemorySize64 = " + VirtualMemorySize64,
			};
			if (File.Exists (directory + file)) {
				FileInfo _FileInfo = new FileInfo (directory + file);
				while (_FileInfo.IsReadOnly) {
					Debug ("Can't write, waiting ..."); 
					Thread.Sleep (1100);
				}
			}
			if (Directory.Exists (Path.GetDirectoryName(directory + file))) {
				try {
					File.WriteAllLines (directory + file, bench);
				} catch {
					Thread.Sleep (500);
				}
			} else {
				Debug ("Bad installation: " + directory + file);
			}
		}
		private static void Main(string[] args) {
			if (args.Contains (argsList[0])) {
				is64 = 1;
			}
			if (File.Exists (KSP_Exe + KSP_Exe_Plat[2]) && !IsKSPLaunched) {
				Debug ("Executing " + KSP_Exe + KSP_Exe_Plat[2]);
				Save (-1, -1, -1, -1);
				exe = Process.Start (KSP_Exe + KSP_Exe_Plat [2], string.Join (" ", args.Except(argsList)));
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
					if ((_now - _idle).TotalSeconds > 60) {
						Debug ("Idle exit.");
						Environment.Exit(0);
					}
					_now = DateTime.Now;
					Thread.Sleep (1000);
				}
			}
			Debug ("Start process information log.");
			//ThGetCPUusage = new Thread (new ThreadStart (GetCPUusage));
			//ThGetCPUusage.Start ();
			directory = Path.GetDirectoryName (exe.MainModule.FileName) + "/";
			Thread.Sleep (1000);
			while (!exe.HasExited) {
				exe.Refresh ();
				Save (GetCPUusage, exe.Threads.Count, exe.WorkingSet64, exe.VirtualMemorySize64);
				Thread.Sleep (1000);
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