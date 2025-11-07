using MediaModel;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MediaModel.Migrations;
using LibServer.Data;



namespace LibServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookSeedController(BooksContext context, IHostEnvironment environment) : ControllerBase
    {
        string _pathName = Path.Combine(environment.ContentRootPath, "Data/audiobooks.csv"); 

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Book")]
        public async Task<ActionResult> PostBook()
        {

            Dictionary<string, Book> books = await context.Books.AsNoTracking().
            ToDictionaryAsync(c => c.Title, StringComparer.OrdinalIgnoreCase);

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
            }; 
            using StreamReader reader = new(_pathName);
            using CsvReader csv = new(reader, config);
            List<Audiobookscsv> records = csv.GetRecords<Audiobookscsv>().ToList();

            foreach(Audiobookscsv record in records)
            {
                if (!books.ContainsKey(record.title))
                {
                    Book book = new()
                    {
                        Title = record.title,
                        Author = record.author,
                        Language = record.language,
                        Publisher = record.publisher,

                    };
                    books.Add(book.Title, book);
                    await context.Books.AddAsync(book); 
                }
            }

            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("Audiobook")]
        public async Task<ActionResult> PostAudiobook()
        {
            Dictionary<string, Book> Books = await context.Books.AsNoTracking().
            ToDictionaryAsync(c => c.Title, StringComparer.OrdinalIgnoreCase);

            CsvConfiguration config = new(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                HeaderValidated = null
            };

            using StreamReader reader = new(_pathName);
            using CsvReader csv = new(reader, config);
            List<Audiobookscsv> records = csv.GetRecords<Audiobookscsv>().ToList();

            int Audiobookcount = 0;

            foreach (Audiobookscsv record in records)
            {
                if (Books.TryGetValue(record.title, out Book? book))
                {
                    Audiobook audiobook = new()
                    {
                        Id = (int)book.Id,
                        //Title = record.title,
                        //Author = record.author,
                        Language = record.language,
                        Narrator = record.narrator,
      
                    };
                    await context.Audiobooks.AddAsync(audiobook);
                    //Audiobookcount++;
                }
            }

            await context.SaveChangesAsync();
            return Ok();
        }

    }
}
