using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace HearthstoneCardsWatcher
{
	public partial class Form1 : Form
	{
		private ConcurrentBag<CardInfo> mInfos = new ConcurrentBag<CardInfo>();
		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
		}

		void mWather_Exception(Exception e)
		{
			MessageBox.Show(e.Message);
		}

		async void mWather_FileChanged(string text)
		{
		}
	}


}
