using System;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace MemoryUsage {
	internal class LAN {

		private static TcpClient TCPClient;
		private static NetworkStream ClientStream;

		private static Thread LANThread;
		private static readonly string ip = "127.0.0.1";

		private static int port {
			get {
				return Settings.Instance.LANport;
			}
		}

		internal static void Toggle() {
			if (Check.isActive) {
				if (LANThread != null) {
					if (LANThread.IsAlive) {
						return;
					}
				}
				LANThread = new Thread (new ThreadStart (GetUsage));
				LANThread.Start ();
				MU.Warning ("Check LAN Started");
			} else {
				if (LANThread != null) {
					if (LANThread.IsAlive) {
						LANThread.Abort ();
						LANThread = null;
					}
				}
				MU.Warning ("Check LAN Stopped");
			}
		}

		private static void Close() {
			try {
			if (ClientStream != null) {
				ClientStream.Close ();
				ClientStream = null;
			}
			if (TCPClient != null) {
				if (TCPClient.Connected) {
					TCPClient.Close ();
				}
				TCPClient = null;
			}
			} catch {
			}
		}

		private static void GetUsage() {
			try {
				//MU.Warning ("LAN Thread Started");
				while (LANThread.IsAlive && Check.isActive) {
					Close();
					TCPClient = new TcpClient (ip, port);
					ClientStream = TCPClient.GetStream ();
					while (TCPClient.Connected && Check.isActive) {
						if (ClientStream.CanRead) {
							Byte[] _data = new Byte[256];
							string _responseData = String.Empty;
							Int32 bytes = ClientStream.Read(_data, 0, _data.Length);
							_responseData = Encoding.ASCII.GetString(_data, 0, bytes);
							string[] _responseDatas = _responseData.Split(' ');
							Check.ClientVERSION = _responseDatas[0];
							Check.CPU.Current = int.Parse (_responseDatas[1]);
							Check.Threads.Current = int.Parse (_responseDatas[2]);
							Check.Physical.Current = Int64.Parse (_responseDatas[3]);
							Check.Virtual.Current = Int64.Parse (_responseDatas[4]);
							Check.lastCheck = DateTime.Now;
							Check.bug = 0;
						}
					}
					Close();
					Thread.Sleep(Settings.Instance.Refresh * 1000);
				}
			}
			catch {
				Close ();
			}
			finally {
				Close ();
				//MU.Warning ("LAN Thread Ended");
			}
		}
	}
}

