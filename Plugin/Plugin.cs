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
	[KSPAddon(KSPAddon.Startup.EveryScene, false)]
	public class MemoryUsage : MU {

		private string Alarm_SoundPath = MOD + "/Sounds/Alarm";
		private AudioSource Alarm;
		private DateTime holdKey = DateTime.Now;

		private void Awake() {
			Settings.Instance.Load ();
			Alarm = gameObject.AddComponent<AudioSource> ();
			if (GameDatabase.Instance.ExistsAudioClip (Alarm_SoundPath)) {
				Alarm.clip = GameDatabase.Instance.GetAudioClip (Alarm_SoundPath);
			}
		}

		private void Start() {
			GUI.Start ();
			Check.Init ();
			if (Settings.Instance.AlarmMEM) {
				Check.Toggle ();
			}
		}

		private void Update() {
			Check.UpdateFPS ();
			if (Settings.Instance.AlarmMEM) {
				if (Check.Physical.Current >= Settings.Instance.MaxMEM && !Check.isEnabled) {
					Check.isEnabled = true;
					if (Settings.Instance.AlarmWithSound && Alarm.clip.isReadyToPlay) {
						Alarm.Play ();
					}
				}
			}
			if (Input.GetKeyDown (Settings.Instance.Key)) {
				Check.isEnabled = !Check.isEnabled;
				Check.Toggle ();
				if (!Check.isEnabled) {
					Check.isBench = false;
				}
				return;
			}
			if (Input.GetKey (Settings.Instance.Key)) {
				if (!Check.isBench && (DateTime.Now - holdKey).TotalSeconds >= 5) {
					Check.isBench = true;
					Check.isEnabled = true;
					Check.Toggle ();
					holdKey = DateTime.Now;
				}
			} else {
				holdKey = DateTime.Now;
			}
		}

		private void OnGUI() {
			GUI.OnGUI ();
		}
	}
}