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
using System.Timers;
using UnityEngine;

namespace MemoryUsage {
	internal class Check {

		private static Timer timer = new Timer(Settings.Instance.Refresh * 1000);

		internal static int bug = 0;

		[KSPField(isPersistant = true)]
		public static bool isEnabled = false;
		[KSPField(isPersistant = true)]
		public static bool isBench = false;

		[KSPField(isPersistant = true)]
		public static string ClientVERSION = "0";
		[KSPField(isPersistant = true)]
		public static Data FPS = new Data();
		[KSPField(isPersistant = true)]
		public static Data CPU = new Data ();
		[KSPField(isPersistant = true)]
		public static Data Threads = new Data ();
		[KSPField(isPersistant = true)]
		public static Data Physical = new Data ();
		[KSPField(isPersistant = true)]
		public static long Virtual = 0;
		[KSPField(isPersistant = true)]
		internal static DateTime lastCheck = DateTime.Now;
		[KSPField(isPersistant = true)]
		private static int lastFrameCount = 0;
		[KSPField(isPersistant = true)]
		private static float lastTimeStartup = 0;
		[KSPField(isPersistant = true)]
		private static DateTime lastFPSDate = DateTime.Now;

		internal static bool isActive {
			get {
				return isEnabled || Settings.Instance.AlarmMEM;
			}
		}

		internal static bool ValuesAreNotUsable {
			get {
				return (CPU.Current == -1 && Threads.Current == -1 && Physical.Current == -1 && Virtual == -1);
			}
		}
		internal static bool ValuesAreNull {
			get {
				return (CPU.Current == 0 && Threads.Current == 0 && Physical.Current == 0 && Virtual == 0 && ClientVERSION == "0");
			}
		}

		internal static void Init() {
			timer.Elapsed += new ElapsedEventHandler (OnTimer);
		}

		internal static void Toggle() {
			FPS.Reset ();
			CPU.Reset ();
			Threads.Reset ();
			Physical.Reset ();
			timer.Enabled = isActive;
			if (isActive) {
				Load ();
			}
		}

		private static void OnTimer(object sender, ElapsedEventArgs e) {
			if (!isActive) {
				timer.Enabled = false;
				return;
			}
			Load ();
		}

		private static void Load() {
			LogFile.Load ();
			if (bug != 0) {
				return;
			}
			if (ValuesAreNotUsable) {
				bug = 2;
				return;
			}
			if (ValuesAreNull) {
				bug = 1;
				return;
			}
			if (ClientVERSION != MU.VERSION) {
				bug = 3;
				return;
			}
			if ((DateTime.Now - lastCheck).TotalSeconds < 35) {
				bug = 0;
				return;
			}
			bug = 1;
		}

		internal static void UpdateFPS() {
			if ((DateTime.Now - lastFPSDate).TotalMilliseconds > 500) {
				if (lastFrameCount != 0 && lastTimeStartup != 0) {
					float _DeltaTime = Time.realtimeSinceStartup - lastTimeStartup;
					int _DeltaFrame = Time.frameCount - lastFrameCount;
					FPS.Current = Mathf.RoundToInt (_DeltaFrame / _DeltaTime);
				}
				lastFrameCount = Time.frameCount;
				lastTimeStartup = Time.realtimeSinceStartup;
				lastFPSDate = DateTime.Now;
			}
		}
	}
}