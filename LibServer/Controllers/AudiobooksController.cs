using MediaModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LibServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudiobooksController : ControllerBase
    {
        private readonly BooksContext _context;

        public AudiobooksController(BooksContext context)
        {
            _context = context;
        }

        // GET: api/Audiobooks
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Audiobook>>> GetAudiobooks()
        {
            return await _context.Audiobooks.ToListAsync();
        }

        // GET: api/Audiobooks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Audiobook>> GetAudiobook(int id)
        {
            var audiobook = await _context.Audiobooks.FindAsync(id);

            if (audiobook == null)
            {
                return NotFound();
            }

            return audiobook;
        }

        // PUT: api/Audiobooks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutAudiobook(int id, Audiobook audiobook)
        {
            if (id != audiobook.Id)
            {
                return BadRequest();
            }

            _context.Entry(audiobook).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AudiobookExists(id))
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

        // POST: api/Audiobooks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Audiobook>> PostAudiobook(Audiobook audiobook)
        {
            _context.Audiobooks.Add(audiobook);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAudiobook", new { id = audiobook.Id }, audiobook);
        }

        // DELETE: api/Audiobooks/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteAudiobook(int id)
        {
            var audiobook = await _context.Audiobooks.FindAsync(id);
            if (audiobook == null)
            {
                return NotFound();
            }

            _context.Audiobooks.Remove(audiobook);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AudiobookExists(int id)
        {
            return _context.Audiobooks.Any(e => e.Id == id);
        }
    }
}
