using System;
using System.Net.Http;

namespace SimpleMusicLibraryWebApplication.Helper
{
	public class SongAPI
	{
		public HttpClient Initial()
		{
			var client = new HttpClient();

			client.BaseAddress = new Uri("https://localhost:44363/");

			return client;
		}
	}
}
