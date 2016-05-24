using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using CoreLib.Annotations;
using Newtonsoft.Json;

namespace CoreLib
{
	public class CardInfo : INotifyPropertyChanged
	{
		private string name;
		private Image image;
		private string type;
		private string zone;
		private string owner;
	    private string text;

	    public Guid ID { get; private set; }

		public string Name
		{
			get { return name; }
			private set { name = value; OnPropertyChanged(); }
		}

		public Image Image
		{
			get { return image; }
			private set { image = value; OnPropertyChanged(); }
		}

		public string Type
		{
			get { return type; }
			private set { type = value; OnPropertyChanged(); }
		}

		public string Zone
		{
			get { return zone; }
			set { zone = value; OnPropertyChanged(); }
		}

		public string Owner
		{
			get { return owner; }
			set { owner = value; OnPropertyChanged(); }
		}

	    public string Text
	    {
	        get { return text; }
	        set { text = value; OnPropertyChanged(); }
	    }

	    public CardInfo(string cardName)
		{
			ID = Guid.NewGuid();
			Name = cardName;
			Type = "";
			Image = Images.Delete;
		}

		public async Task<bool> Load()
		{
			try
			{
				var client = new HttpClient();
				var result = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get,
					"https://omgvamp-hearthstone-v1.p.mashape.com/cards/search/" + Name + "?locale=enUS")
				{
					Headers =
					{
						{"X-Mashape-Key", "DYfGYvK5qxmshp1fofyNr9xXsSflp1fIb7xjsnhFBWVWdAIo0C"}
					}
				});

				var stringResult = await result.Content.ReadAsStringAsync();

				var jsonResult = JsonConvert.DeserializeObject(stringResult);
				var cardInfo = ((IEnumerable<dynamic>)jsonResult).FirstOrDefault(c => c.name == Name /*&& c.collectible != null && (bool)c.collectible*/);
				if (cardInfo == null)
					return false;
				Type = cardInfo.type;
			    Text = cardInfo.text ?? "";

				string imageUrl = cardInfo.img;
				var imgData = await client.GetByteArrayAsync(imageUrl);

				using (var stream = new MemoryStream(imgData))
				{
					Image = Image.FromStream(stream);
					//using (var g = Graphics.FromImage(Image = new Bitmap(img.Width, img.Height / 6)))
					//	g.DrawImage(img, 0, 0, new Rectangle(0, img.Height * 6 / 12, img.Width, img.Height / 6), GraphicsUnit.Pixel);
				}
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			var handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
