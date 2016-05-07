using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CoreLib;

namespace CardsWatcherWpf
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		readonly CardsManager mManager = new CardsManager();

		public MainWindow()
		{
			InitializeComponent();

			var mFilterer = new Filterer(mManager.Infos);
			mListBox.DataContext = mFilterer;
			mTopGrid.DataContext = mFilterer;
			TextBlock.DataContext = mFilterer.FilteredInfos;
		}

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			mManager.Infos.Clear();
		}
	}
}
