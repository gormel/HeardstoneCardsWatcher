using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CoreLib
{
	public class NormalFileSystemWatcher
	{
		public string File { get; set; }
		private DateTime mLastAccessTime;
		private long mLastFileLength;

		public delegate void FileChangedHandler(string changedData);
		public event FileChangedHandler FileChanged;

		public delegate void ExceptionHandler(Exception e);
		public event ExceptionHandler Exception;

		public NormalFileSystemWatcher(string file)
		{
			File = file;
			var fileInfo = new FileInfo(file);
			mLastAccessTime = fileInfo.LastWriteTime;
			mLastFileLength = fileInfo.Length;

			WatchingCycle().ContinueWith(t =>
			{
				if (t.IsFaulted)
				{
                    Exception?.Invoke(t.Exception);
                }
			});
		}

		private async Task WatchingCycle()
		{
			while (true)
			{
				await Task.Delay(100);
				var str = await WatchingCycleBody();
				if (!string.IsNullOrEmpty(str))
				{
				    FileChanged?.Invoke(str);
				}
			}
		}

		private async Task<string> WatchingCycleBody()
		{
			await Task.Yield();
			var fileInfo = new FileInfo(File);
			var accessTime = fileInfo.LastWriteTime;
			var fileLength = fileInfo.Length;
			if (mLastAccessTime != accessTime ||
				mLastFileLength != fileLength)
			{
				using (var stream = new FileStream(File, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
				{
					var buffer = new byte[fileLength - mLastFileLength];
					stream.Position = mLastFileLength;
					await stream.ReadAsync(buffer, 0, buffer.Length);
					mLastAccessTime = accessTime;
					mLastFileLength = fileLength;
					return (Encoding.UTF8.GetString(buffer));
				}
			}
			return null;
		}
	}
}
