using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MimeKit;
using Newtonsoft.Json;
using SimpleMusicLibraryWebApplication.Helper;
using SimpleMusicLibraryWebApplication.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleMusicLibraryWebApplication.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		SongAPI _api = new SongAPI();

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}

		public async Task<IActionResult> Index()
		{
			List<SongData> songs			= new List<SongData>();
			HttpClient client				= _api.Initial();
			HttpResponseMessage response	= await client.GetAsync("api/songs");

			if(response.IsSuccessStatusCode)
			{
				var results = response.Content.ReadAsStringAsync().Result;

				songs = JsonConvert.DeserializeObject<List<SongData>>(results);
			}

			return View(songs);
		}

		public async Task<IActionResult> Details(long Id)
		{
			var song						= new SongData();
			HttpClient client				= _api.Initial();
			HttpResponseMessage response	= await client.GetAsync($"api/songs/{Id}");

			if(response.IsSuccessStatusCode)
			{
				var results = response.Content.ReadAsStringAsync().Result;

				song = JsonConvert.DeserializeObject<SongData>(results);
			}

			return View(song);
		}

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(SongData song)
		{
			var formDataContent = new MultipartFormDataContent();

			formDataContent.Add(new StringContent(song.Title), nameof(song.Title));
			formDataContent.Add(new StringContent(song.Artist), nameof(song.Artist));
			formDataContent.Add(new StringContent(song.Album), nameof(song.Album));

			using (var ms = new MemoryStream())
			{
				await song.MusicFile.CopyToAsync(ms);

				var fileBytes		= ms.ToArray();
				var songContent		= new ByteArrayContent(fileBytes);
				var songFileName	= Path.GetFileName(song.MusicFile.FileName);

				songContent.Headers.Add("Content-Type", song.MusicFile.ContentType);
				formDataContent.Add(songContent, nameof(song.MusicFile), songFileName);
				formDataContent.Add(new StringContent(songFileName), nameof(song.FileName));
			}

			using (var client = _api.Initial())
			{
				using(var result = await client.PostAsync("api/songs", formDataContent)){
					if(result.IsSuccessStatusCode){
						return RedirectToAction(nameof(Index));
					}
				}
			}

			return View();
		}

		public async Task<IActionResult> Download(long Id)
		{
			var song						= new SongData();
			HttpClient client				= _api.Initial();
			HttpResponseMessage response	= await client.GetAsync($"api/songs/{Id}");

			if (response.IsSuccessStatusCode)
			{
				var results = response.Content.ReadAsStringAsync().Result;

				song = JsonConvert.DeserializeObject<SongData>(results);

				var filePath = song.FilePath;

				if (filePath == null || !System.IO.File.Exists(filePath))
				{
					return NotFound();
				}

				return PhysicalFile(filePath, MimeTypes.GetMimeType(filePath), song.FileName);
			}

			return View();
		}

		public async Task<ActionResult> Edit(long Id)
		{
			var song						= new SongData();
			HttpClient client				= _api.Initial();
			HttpResponseMessage response	= await client.GetAsync($"api/songs/{Id}");

			if (response.IsSuccessStatusCode)
			{
				var results = response.Content.ReadAsStringAsync().Result;

				song = JsonConvert.DeserializeObject<SongData>(results);
			}

			return View(song);
		}

		[HttpPost]
		public async Task<IActionResult> Edit(SongData song)
		{
			var formDataContent = new MultipartFormDataContent();

			formDataContent.Add(new StringContent(song.Id.ToString()), nameof(song.Id));
			formDataContent.Add(new StringContent(song.Title), nameof(song.Title));
			formDataContent.Add(new StringContent(song.Artist), nameof(song.Artist));
			formDataContent.Add(new StringContent(song.Album), nameof(song.Album));
			formDataContent.Add(new StringContent(song.FileName), nameof(song.FileName));

			using (var client = _api.Initial())
			{
				using (var result = await client.PutAsync($"api/songs/{song.Id}", formDataContent))
				{
					if (result.IsSuccessStatusCode)
					{
						return RedirectToAction(nameof(Index));
					}
				}
			}

			return View();
		}

		public async Task<IActionResult> Delete(long Id)
		{
			var song						= new SongData();
			HttpClient client				= _api.Initial();
			HttpResponseMessage response	= await client.DeleteAsync($"api/songs/{Id}");

			return RedirectToAction(nameof(Index));
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
