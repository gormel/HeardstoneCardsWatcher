using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreLib
{
	public class CardsManager
	{
		private readonly ObservableCollection<CardInfo> mInfos = new ObservableCollection<CardInfo>();

		public ObservableCollection<CardInfo> Infos { get { return mInfos; } } 

		public CardsManager()
		{
			var mWather = new NormalFileSystemWatcher(@"C:\Games\Hearthstone\Hearthstone_Data\output_log.txt");
			mWather.FileChanged += mWather_FileChanged;
			mWather.Exception += mWather_Exception;
		}

		private void mWather_Exception(Exception e)
		{
			Trace.TraceError(e.Message);
		}

		private async void mWather_FileChanged(string text)
		{
			var strings = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

			await Task.WhenAll(strings.Select(ProcessString));
		}

		readonly Regex rx = new Regex(@"\[name=.*((FRIENDLY|OPPOSING) [A-Z]*)? -> ((FRIENDLY|OPPOSING) [A-Z]*)?");

		async Task ProcessString(string s)
		{
			if (!rx.IsMatch(s) || (!s.Contains("FRIENDLY") && !s.Contains("OPPOSING")))
				return;

			var inst = new LogInstance(s);

			CardInfo info = null;

			if (!string.IsNullOrEmpty(inst.From))
				info = mInfos.FirstOrDefault(i => i.Name == inst.Name && inst.From.StartsWith(i.Owner) && inst.From.EndsWith(i.Zone));

			if (info == null)
			{
				info = new CardInfo(inst.Name);
				if (!await info.Load())
					return;
				if (!string.IsNullOrEmpty(inst.From))
				{
					info.Owner = inst.From.Substring(0, 8);
					info.Zone = inst.From.Substring(9);
				}
				else
				{
					info.Owner = "";
					info.Zone = "";
				}
				mInfos.Add(info);
			}

			if (!string.IsNullOrEmpty(inst.To))
			{
				info.Owner = inst.To.Substring(0, 8);
				info.Zone = inst.To.Substring(9);
			}
			else
			{
				info.Owner = "";
				info.Zone = "";
				mInfos.Remove(info);
			}
		}
	}
}
