using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace SimpleMusicLibraryWebApplication.Models
{
	public class SongData
	{
		[Required]
		public long Id { get; set; }
		[Required]
		public string Title { get; set; }
		[Required]
		public string Artist { get; set; }
		[Required]
		public string Album { get; set; }
		[Required]
		public IFormFile MusicFile { get; set; }
		public string FileName { get; set; }
		public string FilePath{ get; set; }
	}
}
