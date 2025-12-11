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
