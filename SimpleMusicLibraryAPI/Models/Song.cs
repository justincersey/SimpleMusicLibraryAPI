using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace SimpleMusicLibraryAPI.Models
{
	public class Song
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public string Artist { get; set; }
		public string Album { get; set; }
		[NotMapped]
		public IFormFile MusicFile { get; set; }
		public string FileName { get; set; }
		[NotMapped]
		public string FilePath
		{
			get{ return Path.GetFullPath($"MusicFiles/{Id}{Path.GetExtension(FileName)}");}
		}
	}
}
