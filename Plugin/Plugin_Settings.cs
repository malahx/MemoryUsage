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
using System.IO;
using UnityEngine;

namespace MemoryUsage {

	public class Settings : MonoBehaviour {

		public readonly static Settings Instance = new Settings();

		internal string File_settings = KSPUtil.ApplicationRootPath + "GameData/" + MU.MOD + "/Config.txt";

		[Persistent]
		internal string Key = "f11";
		[Persistent]
		internal string BinaryUnits = "true";
		[Persistent]
		internal int Refresh = 1;
		[Persistent]
		internal bool NoErrorMSG = false;
		[Persistent]
		internal bool Color = true;
		[Persistent]
		internal string ColorWarning = "#FF0000";
		[Persistent]
		internal string ColorOk = "#00FF00";
		[Persistent]
		internal string ColorDefault = "#FFFF00";
		[Persistent]
		internal int MinFPS = 30;
		[Persistent]
		internal long MaxMEM = 3500000000;
		[Persistent]
		internal int MaxCPU = 90;
		[Persistent]
		internal bool AlarmMEM = true;
		[Persistent]
		internal bool AlarmWithSound = true;

		public void Save() {
			ConfigNode _temp = ConfigNode.CreateConfigFromObject(this, new ConfigNode());
			_temp.Save(File_settings);
		}
		public void Load() {
			if (File.Exists (File_settings)) {
				ConfigNode _temp = ConfigNode.Load (File_settings);
				ConfigNode.LoadObjectFromConfig (this, _temp);
				MU.Log ("Load");
			} else {
				Save ();
			}
		}
	}
}