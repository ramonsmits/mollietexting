using System;
using Exyll.MollieTexting;

namespace Exyll.MollieTextingConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.Write("Enter originator identifier: ");
			var originator = Console.ReadLine();

			Console.Write("Enter recipient number: ");
			var recipient = Console.ReadLine();

			Console.Write("Enter message: ");
			var message = Console.ReadLine();

			var sender = new MollieTxtSender();
			
			sender.Send(
				originator,
				message,
				recipient
				);

			Console.WriteLine("Text message send!");

		}
	}
}
