using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Clipboard = WindowsClipboard;



namespace net_clipboard {
	class Program {

		private static string _lastClipboardContents = null;


		static void Main(string[] args) {
			if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
				Console.WriteLine($"{RuntimeInformation.OSDescription} is not currently supported.");
				Console.WriteLine();
				return;
			}
			int udpPort = 11084;
			UdpClient socket = new UdpClient(udpPort);
			socket.BeginReceive(new AsyncCallback(OnUdpDataRecv), socket);

			IPEndPoint target = new IPEndPoint(IPAddress.Parse("10.10.1.255"), udpPort);

			Console.WriteLine("Monitoring clipboard and network.  CTL-C to exit.");
			//	Monitor the clipboard
			while (true) {
				string clipboardData = Clipboard.GetText();
				if (!string.IsNullOrWhiteSpace(clipboardData) && _lastClipboardContents != clipboardData) {
					_lastClipboardContents = clipboardData;
					byte[] messageBytes = Encoding.UTF8.GetBytes(clipboardData);
					socket.Send(messageBytes, messageBytes.Length, target);
				}

				Thread.Sleep(250);
			}
			//Console.ReadKey();
		}



		static void OnUdpDataRecv(IAsyncResult result) {
			// this is what had been passed into BeginReceive as the second parameter:
			UdpClient socket = result.AsyncState as UdpClient;
			IPEndPoint source = new IPEndPoint(0, 0);

			// get the actual message and fill out the source:
			byte[] messageBytes = socket.EndReceive(result, ref source);
			string message = Encoding.UTF8.GetString(messageBytes);

			// do what you'd like with `message` here:
			Console.WriteLine("Got " + messageBytes.Length + " bytes from " + source);
			Console.WriteLine(message);
			Clipboard.SetText(message);
			_lastClipboardContents = message;

			// schedule the next receive operation once reading is done:
			socket.BeginReceive(new AsyncCallback(OnUdpDataRecv), socket);
		}




	}
}
