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

using System;
using System.Timers;
using UnityEngine;

namespace MemoryUsage {
	[KSPAddon(KSPAddon.Startup.MainMenu | KSPAddon.Startup.EditorAny | KSPAddon.Startup.TrackingStation | KSPAddon.Startup.Flight | KSPAddon.Startup.SpaceCentre, false)]
	public class MemoryUsage : MonoBehaviour {

		public static string VERSION = "1.00";

		private static bool isdebug = true;
		private static bool bug = false;
		private static string file = "GameData/MemoryUsage/PluginData/MemoryUsage/memory.txt";
		private GUIStyle TextStyle = new GUIStyle ();
		private bool BinaryUnits = true;
		private static Timer timer = new Timer(10000);

		[KSPField(isPersistant = true)]
		public static bool isMemoryUsage = false;
		[KSPField(isPersistant = true)]
		public static Int64 WorkingSet64 = 0;
		[KSPField(isPersistant = true)]
		public static Int64 VirtualMemorySize64 = 0;

		private string MsgToDraw = "Physical MEM: \t{0}\nVirtual MEM: \t{1}";
		private string MsgError = "MemoryUsage can't work!\nYou need to restart MemoryUsage.";

		private string unit (double value, bool bin) {
			int _i = 1;
			string[] _units;
			string[] _units_bin = { "1024", "b", "Kib", "Mib", "Gib", "Tib", "Pib", "Eib", "Zib", "Yib" };
			string[] _units_dec = { "1000", "b", "kb", "Mb", "Gb", "Tb", "Pb", "Eb", "Zb", "Yb" };
			if (bin) {
				_units = _units_bin;
			} else {
				_units = _units_dec;
			}
			int _val = int.Parse (_units [0]);
			while (value > _val) {
				value /= _val;
				_i++;
			}
			if (value >= 100) {
				value = Math.Round (value);
			} else if (value >= 10) {
				value = Math.Round (value,1);
			} else {
				value = Math.Round (value, 2);
			}
			return Math.Round (value, 2) + " " + _units [_i];
		}

		private void Awake() {
			TextStyle.wordWrap = true;
			TextStyle.normal.textColor = Color.white;
			timer.Elapsed += new ElapsedEventHandler(OnTimer);
		}
		private void Update() {
			if (Input.GetKeyDown(KeyCode.F11)) {
				timer.Enabled = !isMemoryUsage;
				timer.Start ();
				isMemoryUsage = !isMemoryUsage;
			}
		}
		private void OnTimer(object sender, ElapsedEventArgs e) {
			Load ();
		}
		private void OnGUI() {
			if (isMemoryUsage) {
				string _string;
				if (!bug) {
					_string = string.Format (MsgToDraw, unit(WorkingSet64, BinaryUnits), unit(VirtualMemorySize64, BinaryUnits));
				} else {
					_string = MsgError;
				}
				GUILayout.BeginArea (new Rect (Screen.width - 175, 75, 175, 100));
				GUILayout.Label (_string, TextStyle);
				GUILayout.EndArea ();
			}
		}
		public void Load() {
			if (System.IO.File.Exists (file)) {
				ConfigNode _temp = ConfigNode.Load (file);
				WorkingSet64 = Int64.Parse (_temp.GetValue ("WorkingSet64"));
				VirtualMemorySize64 = Int64.Parse (_temp.GetValue ("VirtualMemorySize64"));
				if ((DateTime.Now - System.IO.File.GetLastWriteTime (file)).TotalSeconds < 25 || (WorkingSet64 == -1 && VirtualMemorySize64 == -1)) {
					bug = false;
					myDebug ("Load");
					return;
				}
			}
			bug = true;
			myDebug (MsgError);
		}
		private static void myDebug(string _string) {
			if (isdebug) {
				Debug.Log ("MemoryUsage(" + VERSION + "): " + _string);
			}
		}
	}
}