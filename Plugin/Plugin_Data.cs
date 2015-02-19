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
	internal class Data {
		private long current = 0;
		internal long Current {
			get {
				return current;
			}
			set {
				if (value > 0) {
					current = value;
					if (value < Min || Min <= 0) {
						Min = value;
					}
					if (value > Max) {
						Max = value;
					}
					avg += value * Settings.Instance.Refresh;
					Time += Settings.Instance.Refresh;
				}
			}
		}
		internal long Min {
			get;
			private set;
		}
		internal long Max {
			get;
			private set;
		}
		internal int Time {
			get;
			private set;
		}
		internal long avg {
			get;
			private set;
		}
		internal long Avg {
			get {
				if (Time <= 0) {
					return 0;
				}
				return Mathf.RoundToInt (avg / Time);
			}
		}
		internal void Reset() {
			current = 0;
			Min = 0;
			Max = 0;
			Time = 0;
			avg = 0;
		}
	}
}