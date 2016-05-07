using System;

namespace CoreLib
{
	public class LogInstance
	{
		public string Name { get; private set; }
		public string From { get; private set; }
		public string To { get; private set; }

		public string Text { get; set; }

		private const string NameStart = "[name=";
		private const string NameEnd = " id=";

		private const string ZonesStart = "]";

		private const string Opposing = "OPPOSING";
		private const string Friendly = "FRIENDLY";

		private const string Arrow = "->";

		public LogInstance(string text)
		{
			Text = text;

			var pos = text.IndexOf(NameStart, StringComparison.Ordinal) + NameStart.Length;
			var name = ParseName(text.Substring(pos));
			pos += name.Length;
			pos = text.IndexOf(ZonesStart, pos, StringComparison.Ordinal) + ZonesStart.Length;

			var ar = text.IndexOf(Arrow, pos, StringComparison.Ordinal);

			while (!char.IsUpper(text[pos]))
				pos++;

			pos = Math.Min(pos, ar);

			var from = ParseFrom(text.Substring(pos));
			pos = ar + Arrow.Length;

			Name = name.Trim();
			From = from.Trim();
			To = ParseFrom(text.Substring(pos)).Trim();
		}

		private string ParseName(string str)
		{
			var end = str.IndexOf(NameEnd, StringComparison.Ordinal);
			return str.Substring(0, end + 1);
		}

		private string ParseFrom(string str)
		{
			var end = 0;
			while (end < str.Length && (char.IsUpper(str[end]) || char.IsWhiteSpace(str[end])))
				end++;
			return str.Substring(0, end);
		}
	}
}
