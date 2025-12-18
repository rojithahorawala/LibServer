using CsvHelper;
using CsvHelper.Configuration;
using LibServer.Data;
using MediaModel;
using MediaModel.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Globalization;



namespace LibServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookSeedController(
        BooksContext context, 
        IHostEnvironment environment,
        IConfiguration configuration,
        RoleManager<IdentityRole> roleManager,
        UserManager<MediaUserModel> userManager
        ) : ControllerBase
    {
        string _pathName = Path.Combine(environment.ContentRootPath, "Data/audiobooks.csv"); 

        // POST: api/Countries
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Book")]
        public async Task<ActionResult> PostBook()
        {

            //Dictionary<string, Book> books = await context.Books.AsNoTracking().
            //ToDictionaryAsync(c => c.Title, StringComparer.OrdinalIgnoreCase);
            
            var existingBooks = await context.Books
                .AsNoTracking()
                .ToListAsync();

            var books = new Dictionary<string, Book>(StringComparer.OrdinalIgnoreCase);

            foreach (var b in existingBooks)
            {
                if (!string.IsNullOrWhiteSpace(b.Title) && !books.ContainsKey(b.Title))
                {
                    books.Add(b.Title, b);
                }
              
            }

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
                        PublicationYear = int.Parse(record.publicationYear)
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


    var existingBooks = await context.Books
        .AsNoTracking()
        .ToListAsync();

    var booksByTitle = new Dictionary<string, Book>(StringComparer.OrdinalIgnoreCase);

    foreach (var b in existingBooks)
    {
        var key = b.Title?.Trim();                

        if (!string.IsNullOrWhiteSpace(key) && !booksByTitle.ContainsKey(key))
        {
            booksByTitle.Add(key, b);
        }
    }

    // 2) CSV config
    CsvConfiguration config = new(CultureInfo.InvariantCulture)
    {
        HasHeaderRecord = true,
        HeaderValidated = null,
        MissingFieldFound = null
    };

    using StreamReader reader = new(_pathName);
    using CsvReader csv = new(reader, config);
    List<Audiobookscsv> records = csv.GetRecords<Audiobookscsv>().ToList();

    int created = 0;

    foreach (var record in records)
    {
        var titleKey = record.title?.Trim();      // 👈 trim CSV title too

        if (!string.IsNullOrWhiteSpace(titleKey) &&
            booksByTitle.TryGetValue(titleKey, out Book? book))
        {
            var audiobook = new Audiobook
            {
                Bookid = book.Id,                 // FK: relational link

                Title = record.title,
                Author = record.author,
                Language = record.language,
                Narrator = record.narrator,
                Publisher = record.publisher,
                PublicationYear = record.publicationYear
            };

            await context.Audiobooks.AddAsync(audiobook);
            created++;
        }
    }

    await context.SaveChangesAsync();
    return Ok(new { created });

        }

        [HttpPost("Users")]
        public async Task<ActionResult> PostUsers()
        {

            string administrator = "administrator";
            string registedUser = "registedUser";

            if (!await roleManager.RoleExistsAsync(administrator))
            {
                await roleManager.CreateAsync(new IdentityRole(administrator));

            }
            if (!await roleManager.RoleExistsAsync(registedUser))
            {
                await roleManager.CreateAsync(new IdentityRole(registedUser));
            }
            MediaUserModel adminUser = new()
            {
                UserName = "admin",
                Email = "srj8994@gmail.com",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            await userManager.CreateAsync(adminUser, configuration["DefaultPassword:admin"]!);
            await userManager.AddToRoleAsync(adminUser, administrator);

            MediaUserModel regularUser = new()
            {
                UserName = "user",
                Email = "user456@gmail.edu",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            string? x  = configuration["DefaultPassword:user"!];
            IdentityResult ir = await userManager.CreateAsync(regularUser, x!);
            await userManager.AddToRoleAsync(regularUser, registedUser);


            return Ok();
        }
    }
}
