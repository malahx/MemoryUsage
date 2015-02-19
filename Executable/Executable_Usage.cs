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

	internal class Usage {

		//private static int CPUusage = 0;
		private static DateTime lastDate = DateTime.Now;
		private static double lastProcessorTime = 0;

		// Don't know why, but on Arch linux 64b, TotalProcessorTime always return 0 with mono 3.12
		private static string[] LinuxCPUusageCLI = { "ps", "h -C {0} -o pcpu" };

		internal static int GetThreads {
			get {
				if (Exe.KSP == null) {
					return 0;
				}
				return Exe.KSP.Threads.Count;
			}
		}

		internal static long GetPhysical {
			get {
				if (Exe.KSP == null) {
					return 0;
				}
				return Exe.KSP.WorkingSet64;
			}
		}

		internal static long GetVirtual {
			get {
				if (Exe.KSP == null) {
					return 0;
				}
				return Exe.KSP.VirtualMemorySize64;
			}
		}

		internal static int GetCPU {
			get {
				if (Exe.KSP == null) {
					return 0;
				}
				try {
					DateTime _currentDate = DateTime.Now;
					double _currentPorcessorTime = Exe.KSP.TotalProcessorTime.TotalMilliseconds;
					int _CPUusage = (int)(100 * (_currentPorcessorTime - lastProcessorTime) / (_currentDate - lastDate).TotalMilliseconds / Environment.ProcessorCount);
					// Don't know why, but on Arch linux 64b, TotalProcessorTime always return 0 with mono 3.12
					if (MU.IsLinux) {
						if (_CPUusage == 0) {
							PerformanceCounter CPUcounter = new PerformanceCounter();
							CPUcounter.CategoryName = "Process";
							CPUcounter.CounterName = "% Processor Time";
							CPUcounter.InstanceName = Exe.KSP.ProcessName;
							_CPUusage = (int)CPUcounter.NextValue();
						}
						if (_CPUusage == 0) {
							ProcessStartInfo _startInfo = new ProcessStartInfo ();
							Process _exe = new Process ();
							_startInfo.FileName = LinuxCPUusageCLI[0];
							_startInfo.Arguments = string.Format (LinuxCPUusageCLI [1], Exe.KSP.ProcessName);
							_startInfo.RedirectStandardOutput = true;
							_startInfo.UseShellExecute = false;
							_startInfo.CreateNoWindow = true;
							_exe.StartInfo = _startInfo;
							_exe.Start();
							string _output = _exe.StandardOutput.ReadToEnd ().Split('.')[0];
							if (!_exe.HasExited) {
								_exe.Kill();
							}
							_exe.Close();
							_CPUusage = (int)(double.Parse(_output) / Environment.ProcessorCount);
							//MU.Log("CPU usage with CLI(" + _startInfo.FileName + " " + _startInfo.Arguments + "): " + _CPUusage + "%");
						} else {
							//MU.Log("CPU usage with PerformanceCounter: " + _CPUusage + "%");
						}
					}
					lastProcessorTime = _currentPorcessorTime;
					lastDate = _currentDate;
					return _CPUusage;
				} catch (Exception e) {
					MU.Log ("ERROR");
					MU.Log ("\tSource: " + e.Source);
					MU.Log ("\tMessage: " + e.Message);
					MU.Log ("\tStackTrace: " + e.StackTrace);
				}
				return 0;
			}
		} 
	}
}