using System;
using System.Linq;
using PCSC;
using PCSC.Iso7816;

namespace ACRBuzzerControl
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			try
			{
				var readerName = GetReaderName();
				if (string.IsNullOrEmpty(readerName))
					return;

				Console.WriteLine($"Using {readerName}");
				var state = GetExpectedState();
				if (!state.HasValue)
					return;

				SetBuzzerState(readerName, state.Value);
			}
			catch (Exception e)
			{
				Console.WriteLine(e.Message);
			}
		}

		private static string GetReaderName()
		{
			using var context = ContextFactory.Instance.Establish(SCardScope.System);

			Console.WriteLine("Connected readers:");
			var readerNames = context
				.GetReaders()
				.Take(10)
				.ToArray();

			var index = 0;
			foreach (var readerName in readerNames) {
				Console.WriteLine($"({index++}) {readerName}");
			}

			Console.Write("\nPlease select a compatible ACR122U device...");
			var input = Console.ReadLine();
			if (!int.TryParse(input, out index))
				return null;

			if (index < 0 || index >= readerNames.Length)
				return null;

			return readerNames[index];
		}

		private static void SetBuzzerState(string readerName, bool enabled)
		{
			Console.Write("Setting buzzer state... ");

			using var ctx = ContextFactory.Instance.Establish(SCardScope.System);
			using var isoReader = new IsoReader(ctx, "ACS ACR1252 Dual Reader PICC 0", SCardShareMode.Shared, SCardProtocol.Any, false);
			var p2 = enabled ? (byte) 0xFF : (byte) 0x00;

			// Set buzzer output during card detection
			var apdu = new CommandApdu(IsoCase.Case2Short, isoReader.ActiveProtocol)
			{
				CLA = 0xFF,         // Class
				Instruction = 0x00, // Insruction
				P1 = 0x52,          // Parameter 1
				P2 = p2,            // Parameter 2 - 00h: Buzzer will NOT turn on when a card is detected
				                    // FFh: Buzzer will turn on when a card is detected
				Le = 0x00           // Expected length of the returned data
			};

			var response = isoReader.Transmit(apdu);
			Console.WriteLine(response.SW1 == 0x90 && response.SW2 == 0x00 ? "Success!" : "ERROR");
		}

		private static bool? GetExpectedState()
		{
			Console.WriteLine("\nExpected state:");
			Console.WriteLine("(0) Buzzer will NOT turn on when a card is detected");
			Console.WriteLine("(1) Buzzer will turn on when a card is detected");

			Console.Write("\nPlease select state...");
			var input = Console.ReadLine();

			return input switch
			{
				"0" => new bool?(false),
				"1" => true,
				_ => null,
			};
		}
	}
}
