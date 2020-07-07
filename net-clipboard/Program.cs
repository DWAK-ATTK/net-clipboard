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

		private object _lastClipboardContents = null;


		static void Main(string[] args) {
			if(!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
				Console.WriteLine($"{RuntimeInformation.OSDescription} is not currently supported.");
				Console.WriteLine();
				return;
			}
			UdpClient socket = new UdpClient(5394);
			socket.BeginReceive(new AsyncCallback(OnUdpDataRecv), socket);

			IPEndPoint target = new IPEndPoint(IPAddress.Parse("10.10.1.255"), 5394);

			//	Monitor the clipboard
			while (true) {
				string clipboardData = Clipboard.GetText();
				if (!string.IsNullOrWhiteSpace(clipboardData)) {

				}

				Thread.Sleep(250);
			}

			// send a couple of sample messages:
			for (int num = 1; num <= 3; num++) {
				string message = $"Hello from {num}.";
				byte[] messageBytes = Encoding.UTF8.GetBytes(message);
				socket.Send(messageBytes, messageBytes.Length, target);
			}

			Console.ReadKey();
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

			// schedule the next receive operation once reading is done:
			socket.BeginReceive(new AsyncCallback(OnUdpDataRecv), socket);
		}




	}
}
