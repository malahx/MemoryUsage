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
using UnityEngine;

namespace MemoryUsage {
	public class MU : MonoBehaviour {

		internal readonly static string VERSION = "1.20";
		internal readonly static string MOD = "MemoryUsage";
		internal static bool isdebug = true;
		internal static void Log(string msg) {
			if (isdebug) {
				Debug.Log (MOD + "(" + VERSION + "): " + msg);
			}
		}
		internal static void Warning(string msg) {
			if (isdebug) {
				Debug.LogWarning (MOD + "(" + VERSION + "): " + msg);
			}
		}
		internal static string unit (double value) {
			return unit (value, Settings.Instance.BinaryUnits);
		}
		internal static string unit (double value, string bin) {
			int _i = 1;
			string[] _units_bin = { "1024", "b", "Kib", "Mib", "Gib", "Tib", "Pib", "Eib", "Zib", "Yib" };
			string[] _units_dec = { "1000", "b", "kb", "Mb", "Gb", "Tb", "Pb", "Eb", "Zb", "Yb" };
			string[] _units = (bool.Parse (bin) ? _units_bin : _units_dec);
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
	}
}