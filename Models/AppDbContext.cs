using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Hakuna.Models;
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options)
            :base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);
            string GenreJSon = System.IO.File.ReadAllText("InitialGenres.json");
            List<Genre>? genres = System.Text.Json.
                JsonSerializer.Deserialize<List<Genre>>(GenreJSon);
//Seed to categorie
            foreach (Genre c in genres)
                model.Entity<Genre>()
                    .HasData(c);
        }
        public DbSet<Movie> Movies {get;set;}
        public DbSet<Genre> Genres {get;set;}
        public DbSet<Membership> Membershiptypes {get;set;}
        public DbSet<Customer> Customers{get;set;}

    }
