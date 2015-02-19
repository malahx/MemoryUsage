using System;
using System.Threading;
using UnityEngine;

namespace MemoryUsage {
	public class FPS {

		private static int Refresh = 500;
		private static Thread FPSThread;



		internal static void Toggle() {
			if (Check.isActive) {
				if (FPSThread != null) {
					if (FPSThread.IsAlive) {
						return;
					}
				}
				FPSThread = new Thread (new ThreadStart (GetFPS));
				FPSThread.Start ();
			} else {
				if (FPSThread != null) {
					if (FPSThread.IsAlive) {
						FPSThread.Abort ();
						FPSThread = null;
					}
				}
			}
		}

		internal static void GetFPS() {
			try {
				//MU.Warning ("FPS Thread Started");
				while (FPSThread.IsAlive && Check.isActive) {
					int _lastFrameCount = Time.frameCount;
					float _lastTimeStartup = Time.realtimeSinceStartup;
					Thread.Sleep(Refresh);
					float _DeltaTime = Time.realtimeSinceStartup - _lastTimeStartup;
					int _DeltaFrame = Time.frameCount - _lastFrameCount;
					Get.Current = Mathf.RoundToInt(_DeltaFrame / _DeltaTime);
				}
			}
			finally {
				//MU.Warning ("FPS Thread Ended");
			}
		}
	}
}