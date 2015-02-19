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
	internal class GUI {

		private static GUIStyle TextStyle = new GUIStyle ();

		private static Rect AreaRect = new Rect (Screen.width - 200, 175, 200, Screen.height - 175);

		private static string MsgFPS = 		"FPS: \t\t\t\t<b><color={1}>{0}</color></b>";
		private static string MsgCPU = 		"CPU Usage: \t\t<b><color={1}>{0}</color></b> (<b><color={3}>{2}</color></b>)";
		private static string MsgPhysical = "Physical MEM: \t<b><color={1}>{0}</color></b>";
		private static string MsgVirtual = 	"Virtual MEM: \t<b><color={1}>{0}</color></b>";
		private static string MsgStats = 	"Min(<b><color={1}>{0}</color></b>) Avg(<b><color={3}>{2}</color></b>) Max(<b><color={5}>{4}</color></b>)";
		private static string MsgStatsMem = "Avg(<b><color={1}>{0}</color></b>) Max(<b><color={3}>{2}</color></b>)";

		private static string MsgWait = 	"<b><color={0}>You need to wait a little!</color></b>";
		private static string MsgError = 	"<b><color={0}>MemoryUsage can't work!" + Environment.NewLine + "You need to restart MemoryUsage.exe</color></b>";
		private static string MsgInstall = 	"<b><color={0}>MemoryUsage can't work!" + Environment.NewLine + "You need to reinstall MemoryUsage</color></b>";

		private enum GetColor {
			DEF,
			FPS,
			CPU,
			MEM
		}

		private static string color() {
			return Settings.Instance.ColorDefault;
		}

		private static string color(long i, GetColor type) {
			if (!Settings.Instance.Color) {
				return Settings.Instance.ColorDefault;
			}
			if (type == GetColor.DEF) {
				return Settings.Instance.ColorDefault;
			} else if (type == GetColor.CPU) {
				if (i > Settings.Instance.MaxCPU) {
					return Settings.Instance.ColorWarning;
				}
				return Settings.Instance.ColorOk;
			} else if (type == GetColor.FPS) {
				if (i < Settings.Instance.MinFPS) {
					return Settings.Instance.ColorWarning;
				}
				return Settings.Instance.ColorOk;
			} else if (type == GetColor.MEM) {
				if (i > Settings.Instance.MaxMEM) {
					return Settings.Instance.ColorWarning;
				}
				return Settings.Instance.ColorOk;
			}
			return Settings.Instance.ColorDefault;
		}

		internal static void Start() {
			TextStyle.wordWrap = true;
			TextStyle.normal.textColor = Color.white;
		}

		internal static void OnGUI() {
			if (Check.isEnabled) {
				string _string = string.Format (MsgFPS, Check.FPS.Current, color(Check.FPS.Current, GetColor.FPS));
				_string += Environment.NewLine;
				if (Check.bug == 0) {
					string _CPU = string.Format (MsgCPU, (Check.CPU.Current != 0 ? Check.CPU.Current + "%" : "N/A"), color(Check.CPU.Current, GetColor.CPU), Check.Threads.Current, color());
					string _PHY = string.Format (MsgPhysical, MU.unit (Check.Physical.Current), color(Check.Physical.Current, GetColor.MEM));
					string _VIR = string.Format (MsgVirtual, MU.unit (Check.Virtual), color());
					if (!Check.isBench) {
						_string += _CPU;
						_string += Environment.NewLine;
						_string += _PHY;
						_string += Environment.NewLine;
						_string += _VIR;
					} else {
						_string += string.Format (MsgStats, Check.FPS.Min, color(Check.FPS.Min, GetColor.FPS), Check.FPS.Avg, color(Check.FPS.Avg, GetColor.FPS), Check.FPS.Max, color(Check.FPS.Max, GetColor.FPS));
						_string += Environment.NewLine;
						_string += Environment.NewLine;
						_string += _CPU;
						_string += Environment.NewLine;
						_string += string.Format (MsgStats, (Check.CPU.Min != 0 ? Check.CPU.Min + "%" : "N/A"), color(Check.CPU.Min, GetColor.CPU), (Check.CPU.Avg != 0 ? Check.CPU.Avg + "%" : "N/A"), color(Check.CPU.Avg, GetColor.CPU), (Check.CPU.Max != 0 ? Check.CPU.Max + "%" : "N/A"), color(Check.CPU.Max, GetColor.CPU));
						_string += Environment.NewLine;
						_string += Environment.NewLine;
						_string += _PHY;
						_string += Environment.NewLine;
						_string += string.Format (MsgStatsMem, MU.unit (Check.Physical.Avg), color(Check.Physical.Avg, GetColor.MEM), MU.unit (Check.Physical.Max), color(Check.Physical.Max, GetColor.MEM));
						_string += Environment.NewLine;
						_string += Environment.NewLine;
						_string += _VIR;
					}
				} else if (Check.bug == 1) {
					if (!Settings.Instance.NoErrorMSG) {
						_string += string.Format (MsgError, Settings.Instance.ColorWarning);
					}
				} else if (Check.bug == 2) {
					if (!Settings.Instance.NoErrorMSG) {
						_string += string.Format (MsgWait, Settings.Instance.ColorWarning);
					}
				} else if (Check.bug == 3) {
					if (!Settings.Instance.NoErrorMSG) {
						_string += string.Format (MsgInstall, Settings.Instance.ColorWarning);
					}
				}
				GUILayout.BeginArea (AreaRect);
				GUILayout.BeginVertical ();
				GUILayout.BeginHorizontal ();
				GUILayout.Label (_string, TextStyle);
				GUILayout.EndHorizontal ();
				GUILayout.EndVertical ();
				GUILayout.EndArea ();
			}
		}
	}
}