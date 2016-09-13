using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using CoreLib;
using CoreLib.Annotations;

namespace CardsWatcherWpf
{
	public class Filterer : INotifyPropertyChanged
	{
		private bool mOwnerFriendly = true;
		private bool mOwnerOpposing = true;

		private bool mZoneDeck = true;
		private bool mZoneHand = true;
		private bool mZonePlay = true;
		private bool mZoneGrav = true;

	    private string mFilterString = "";

		public ListCollectionView FilteredInfos { get; private set; }

		#region FilterProps

		public bool OwnerFriendly
		{
			get { return mOwnerFriendly; }
			set
			{
				mOwnerFriendly = value;
				FilteredInfos.Refresh();
			}
		}

		public bool OwnerOpposing
		{
			get { return mOwnerOpposing; }
			set
			{
				mOwnerOpposing = value;
				FilteredInfos.Refresh();
			}
		}

		public bool ZoneDeck
		{
			get { return mZoneDeck; }
			set
			{
				mZoneDeck = value;
				FilteredInfos.Refresh();
			}
		}

		public bool ZoneHand
		{
			get { return mZoneHand; }
			set
			{
				mZoneHand = value;
				FilteredInfos.Refresh();
			}
		}

		public bool ZonePlay
		{
			get { return mZonePlay; }
			set
			{
				mZonePlay = value;
				FilteredInfos.Refresh();
			}
		}

		public bool ZoneGrav
		{
			get { return mZoneGrav; }
			set
			{
				mZoneGrav = value;
				FilteredInfos.Refresh();
			}
		}

	    public string FilterString
	    {
	        get { return mFilterString; }
	        set
	        {
	            mFilterString = value;
	            FilteredInfos.Refresh();
	        }
	    }

		#endregion


		public Filterer(ObservableCollection<CardInfo> infos)
		{
			FilteredInfos = (ListCollectionView)CollectionViewSource.GetDefaultView(infos);
			FilteredInfos.IsLiveFiltering = true;
			foreach (var prop in typeof(CardInfo).GetProperties())
			{
				FilteredInfos.LiveFilteringProperties.Add(prop.Name);
			}
			FilteredInfos.Filter = o => FilterOwner(o as CardInfo) && FilterZone(o as CardInfo) && FilterByString(o as CardInfo);
            FilteredInfos.SortDescriptions.Add(new SortDescription(nameof(CardInfo.Cost), ListSortDirection.Ascending));
		}

		#region Filters

		private bool FilterOwner(CardInfo info)
		{
			if (OwnerFriendly && info.Owner == "FRIENDLY")
				return true;

			if (OwnerOpposing && info.Owner == "OPPOSING")
				return true;

			return false;
		}

		private bool FilterZone(CardInfo info)
		{

			if (ZoneDeck && info.Zone == "DECK")
				return true;

			if (ZoneHand && info.Zone == "HAND")
				return true;

			if (ZonePlay && info.Zone == "PLAY")
				return true;

			if (ZoneGrav && info.Zone == "GRAVEYARD")
				return true;

			return false;
		}

	    private bool FilterByString(CardInfo info)
	    {
	        if (string.IsNullOrEmpty(FilterString))
	            return true;

	        var keywords = FilterString.Split(new[] {" ", "\t"}, StringSplitOptions.RemoveEmptyEntries).Select(s => s.ToLower()).ToArray();

	        if (keywords.Any(s => info.Name.ToLower().Contains(s)))
	            return true;

	        if (keywords.Any(s => info.Type.ToLower().Contains(s)))
	            return true;

	        if (keywords.Any(s => info.Text.ToLower().Contains(s)))
	            return true;

	        return false;
	    }

		#endregion

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
	}
}
