using LibraryApp.Models;                                    // THIS LINE WAS MISSING
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) 
        { }

        public DbSet<Book> Books => Set<Book>();   // Now it finds Book class
    }
}