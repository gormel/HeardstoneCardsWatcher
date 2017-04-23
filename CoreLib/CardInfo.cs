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
using Newtonsoft.Json.Linq;

namespace CoreLib
{
	public class CardInfo : INotifyPropertyChanged
	{
	    private const string HeroPowerType = "Hero Power";

		private string mName;
		private Image mImage;
		private string mType;
		private string mZone;
		private string mOwner;
	    private string mText;
	    private int mCost;

	    public Guid ID { get; private set; }

		public string Name
		{
			get { return mName; }
			private set { mName = value; OnPropertyChanged(); }
		}

		public Image Image
		{
			get { return mImage; }
			private set { mImage = value; OnPropertyChanged(); }
		}

		public string Type
		{
			get { return mType; }
			private set { mType = value; OnPropertyChanged(); }
		}

		public string Zone
		{
			get { return mZone; }
			set { mZone = value; OnPropertyChanged(); }
		}

		public string Owner
		{
			get { return mOwner; }
			set { mOwner = value; OnPropertyChanged(); }
		}

	    public string Text
	    {
	        get { return mText; }
	        set { mText = value; OnPropertyChanged(); }
	    }

	    public int Cost
	    {
	        get { return mCost; }
	        set { mCost = value; OnPropertyChanged(); }
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
				var cardInfo = ((IEnumerable<dynamic>)jsonResult).FirstOrDefault(c => c.name== Name);
				if (cardInfo == null)
					return false;
				Type = cardInfo.type;
			    if (Type == HeroPowerType)
			        return false;
			    Text = cardInfo.text ?? "";
			    Cost = cardInfo.cost;

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
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
	}
}
