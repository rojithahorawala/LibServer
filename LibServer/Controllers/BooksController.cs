using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediaModel;
using LibServer.DTO;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace LibServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(BooksContext context) : ControllerBase
    {

        // GET: api/Books
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
        {
            return await context.Books.ToListAsync();

        }

        // GET: api/Books
        [HttpGet("inventory")]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventory()
        {
            var result =  await context.Books.
                Select(c => new Inventory()
                {
                    Id = c.Id,
                    Title = c.Title,
                    PublicationYear = c.PublicationYear,
                    Author = c.Author,
                    SumOfBooks = context.Books.Count(b => b.Title == c.Title)
                }).
                ToListAsync();
            return Ok(result);
        }
        

        // GET: api/Books/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            return book;
        }


        [HttpGet("Inventory/{id}")]
        public ActionResult<Inventory> GetInventory(int id)
        {
            var book = context.Books.Select(book => new Inventory
            {
                Id = book.Id,
                Title = book.Title,
                PublicationYear = book.PublicationYear,
                Author = book.Author,
                SumOfBooks = context.Books.Count(b => b.Title == book.Title)
            });


          return context.Books.Select(book => new Inventory
          {
              Id = book.Id,
              Title = book.Title,
              PublicationYear = book.PublicationYear,
              Author = book.Author,
              SumOfBooks = context.Books.Count(b => b.Title == book.Title)
          }).Single(c => c.Id == id);

        }



        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            if (id != book.Id)
            {
                return BadRequest();
            }

            context.Entry(book).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
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

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            context.Books.Add(book);
            await context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.Id }, book);
        }

        // DELETE: api/Books/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            context.Books.Remove(book);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool BookExists(int id)
        {
            return context.Books.Any(e => e.Id == id);
        }
    }
}
