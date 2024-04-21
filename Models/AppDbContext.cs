using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using System.Runtime.CompilerServices;


namespace Hakuna.Models;
    public class AppDbContext : DbContext
    {
        private string connectionString; 
        const string databaseName = "DotNetY";

        public AppDbContext(DbContextOptions options)
            :base(options)
        {
            #pragma warning disable EF1001 // Internal EF Core API usage.
            var extension = options.FindExtension<SqlServerOptionsExtension>()!;
            connectionString = extension.ConnectionString!;
            connectionString = connectionString!.Replace("Database=" + databaseName+";", "");
                        Console.WriteLine(connectionString);

            InitializeDatabase();
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
        /**
     * this function is made for those who run this app first time in their local repository
     * it's to check if there a database under name "CarpooliDotTn" or no
     * if it does not exist it creates a one before connecting to it without any errors and exceptions
     * otherwise it does nothing. keep it in case someone tries to download and try it himself
     */
  
    public void InitializeDatabase()
    {
        void CreateDatabase(SqlConnection connection, string databaseName)
        {
            using (SqlCommand command = new SqlCommand($"CREATE DATABASE {databaseName}", connection))
            {
                command.ExecuteNonQuery();
            }
        }
        bool DatabaseExists(SqlConnection connection, string databaseName)
        {
            using (SqlCommand command = new SqlCommand($"IF DB_ID('{databaseName}') IS NOT NULL SELECT 1 ELSE SELECT 0", connection))
            {
                return (int)command.ExecuteScalar() == 1;
            }
        }
        try
        {
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                
                connection.Open();
                // Check if the database exists
                if (DatabaseExists(connection, databaseName))
                {
                    Console.WriteLine("Database check completed, no actions needed.");
                }
                else
                {
                    // Create the database
                    CreateDatabase(connection, databaseName);
                    Console.WriteLine("Database not found, new one has been created.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    }
