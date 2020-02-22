using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleMusicLibraryAPI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMusicLibraryAPI.Controllers
{
    [Route("api/Songs")]
    [ApiController]
    public class SongsController : ControllerBase
    {
        private readonly SimpleMusicLibraryContext _context;

        public SongsController(SimpleMusicLibraryContext context)
        {
            _context = context;
        }

        // GET: api/Songs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Song>>> GetSongs()
        {
            return await _context.Songs.ToListAsync();
        }

        // GET: api/Songs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Song>> GetSong(long id)
        {
            var song = await _context.Songs.FindAsync(id);

            if (song == null)
            {
                return NotFound();
            }

            return song;
        }

        // PUT: api/Songs/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSong(long id, [FromForm]Song song)
        {
            if (id != song.Id)
            {
                return BadRequest();
            }

            _context.Entry(song).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SongExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Songs
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Song>> PostSong([FromForm]Song song)
        {
            //Get music file
            var musicFile   = song.MusicFile;
            string fileName = "song.mp3";
            
            if(musicFile != null){
                fileName = musicFile.FileName;

                song.FileName = fileName;
            }
            
            _context.Songs.Add(song);
            await _context.SaveChangesAsync();

            //Save file
            if (musicFile != null && musicFile.Length > 0)
            {
                var filePath = $"MusicFiles/{song.Id}{Path.GetExtension(fileName)}";

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await musicFile.CopyToAsync(fileStream);
                }
            }

            //clear the uploaded file so it isn't returned in the response
            song.MusicFile = null;

            return CreatedAtAction(nameof(GetSong), new { id = song.Id }, song);
        }

        // DELETE: api/Songs/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Song>> DeleteSong(long id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null)
            {
                return NotFound();
            }

            _context.Songs.Remove(song);
            await _context.SaveChangesAsync();

            try{
                System.IO.File.Delete(song.FilePath);
            }catch(Exception ex){
                //TODO: handle exceptions
                throw;
            }
            

            return song;
        }

        private bool SongExists(long id)
        {
            return _context.Songs.Any(e => e.Id == id);
        }
    }
}
