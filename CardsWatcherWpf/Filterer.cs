using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using CoreLib;
using CoreLib.Annotations;

namespace CardsWatcherWpf
{
	public class Filterer : INotifyPropertyChanged
	{
		private bool ownerFriendly = true;
		private bool ownerOpposing = true;

		private bool zoneDeck = true;
		private bool zoneHand = true;
		private bool zonePlay = true;
		private bool zoneGrav = true;

		private bool typeSpell = true;
		private bool typeMinion = true;
		private bool typeWeapon = true;

		public ListCollectionView FilteredInfos { get; private set; }

		#region FilterProps

		public bool OwnerFriendly
		{
			get { return ownerFriendly; }
			set
			{
				ownerFriendly = value;
				FilteredInfos.Refresh();
			}
		}

		public bool OwnerOpposing
		{
			get { return ownerOpposing; }
			set
			{
				ownerOpposing = value;
				FilteredInfos.Refresh();
			}
		}

		public bool ZoneDeck
		{
			get { return zoneDeck; }
			set
			{
				zoneDeck = value;
				FilteredInfos.Refresh();
			}
		}

		public bool ZoneHand
		{
			get { return zoneHand; }
			set
			{
				zoneHand = value;
				FilteredInfos.Refresh();
			}
		}

		public bool ZonePlay
		{
			get { return zonePlay; }
			set
			{
				zonePlay = value;
				FilteredInfos.Refresh();
			}
		}

		public bool ZoneGrav
		{
			get { return zoneGrav; }
			set
			{
				zoneGrav = value;
				FilteredInfos.Refresh();
			}
		}

		public bool TypeSpell
		{
			get { return typeSpell; }
			set
			{
				typeSpell = value;
				FilteredInfos.Refresh();
			}
		}

		public bool TypeMinion
		{
			get { return typeMinion; }
			set
			{
				typeMinion = value;
				FilteredInfos.Refresh();
			}
		}

		public bool TypeWeapon
		{
			get { return typeWeapon; }
			set
			{
				typeWeapon = value;
				FilteredInfos.Refresh();
			}
		}

		#endregion


		public Filterer(ObservableCollection<CardInfo> infos)
		{
			FilteredInfos = CollectionViewSource.GetDefaultView(infos) as ListCollectionView;
			FilteredInfos.IsLiveFiltering = true;
			foreach (var prop in typeof(CardInfo).GetProperties())
			{
				FilteredInfos.LiveFilteringProperties.Add(prop.Name);
			}
			FilteredInfos.Filter = o => FilterOwner(o as CardInfo) && FilterZone(o as CardInfo) && FilterType(o as CardInfo);
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

		private bool FilterType(CardInfo info)
		{
			if (TypeSpell && info.Type == "Spell")
				return true;

			if (TypeMinion && info.Type == "Minion")
				return true;

			if (TypeWeapon && info.Type == "Weapon")
				return true;

			return false;
		}

		#endregion

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
