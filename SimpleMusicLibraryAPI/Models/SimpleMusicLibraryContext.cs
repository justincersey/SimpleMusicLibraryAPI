using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SimpleMusicLibraryAPI.Models
{
	public class SimpleMusicLibraryContext:DbContext
	{
		public SimpleMusicLibraryContext(DbContextOptions<SimpleMusicLibraryContext> options):base(options){ }

		public DbSet<Song> Songs { get; set; }
	}
}
