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

// usage:	MemoryUsage.exe [-64b] [ksp_args]

// example:	MemoryUsage.exe -64b -force-opengl
//			It will start KSP in 64 bits with opengl


using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MemoryUsage {
	internal class LAN {

		private static Thread LANThread;
		internal static string ip = "127.0.0.1";
		internal static int port = 17565;
		private static TcpListener tcpListener = null;
		private static TcpClient client = null;

		internal static void Init() {
			tcpListener = new TcpListener(IPAddress.Parse(ip), port);
			tcpListener.Start ();
			MU.Log (string.Format("Server started: {0}:{1}", ip, port));
		}

		internal static void Send() {
			if (LANThread == null) {
				LANThread = new Thread (new ThreadStart (ThreadSend));
				LANThread.Start ();
			}
		}

		private static void ThreadSend() {
			try {
				if (client != null) {
					if (client.Connected) {
						try {
							NetworkStream _clientStream = client.GetStream ();
							ASCIIEncoding _encoder = new ASCIIEncoding ();
							string _Data = string.Format ("{0} {1} {2} {3} {4}", MU.VERSION, Usage.GetCPU, Usage.GetThreads, Usage.GetPhysical, Usage.GetVirtual);
							byte[] _buffer = _encoder.GetBytes (_Data);
							_clientStream.Write (_buffer, 0, _buffer.Length);
							_clientStream.Flush ();
							//MU.Log ("Data Sended: " + _Data);
						} catch {
							MU.Log ("LAN Send Error");
						}
					} else {
						client.Close ();
						client = null;
					}
				} else {
					MU.Log ("Waiting a connection ...");
					client = tcpListener.AcceptTcpClient ();
					MU.Log ("Connection Accepted");
				}
			} finally {
				LANThread = null;
			}
		}
	}
}