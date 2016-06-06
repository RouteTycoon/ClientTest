using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TMR;

namespace ClientTest
{
	class Program
	{
		static void Main(string[] args)
		{
			enterHostName:
			string name;
			ushort port;

			Console.Write("Server Address> ");
			name = Console.ReadLine().Trim();

			if (string.IsNullOrEmpty(name))
			{
				Console.WriteLine("Please re-enter.");
				goto enterHostName;
			}

			enterHostPort:
			Console.Write("Server Port> ");
			try
			{
				port = Convert.ToUInt16(Console.ReadLine().Trim());
			}
			catch
			{
				Console.WriteLine("Please re-enter.");
				goto enterHostPort;
			}

			Client c = new Client();
			c.Guid = "Tester";
			c.ReceiveMessage += new MessageEventHandler((e) => Console.WriteLine(e.Message.Text));

			Console.WriteLine("Connecting...");

			c.Start(name, port);
			if (!c.isConnected)
			{
				Console.WriteLine("Connection Failed");
				Console.ReadLine();
				return;
			}

			while (true)
			{
				string msg;
				Console.Write("Message (Exit: X)> ");
				msg = Console.ReadLine().Trim();

				if (string.IsNullOrEmpty(msg))
					continue;

				if(msg.Equals("X"))
				{
					c.Stop();
					return;
				}

				c.SendToServer(new Message() { Text = $"<{c.Guid}> " + msg, Type = MessageType.Chat });
			}
		}

		private static void C_Kicked(TMR.KickEventArgs e)
		{
			Console.WriteLine("Kicked! - {0}", e.Reason);
			Console.ReadLine();
		}
	}
}
