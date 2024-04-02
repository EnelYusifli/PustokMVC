using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PustokTemp.Models;

namespace PustokTemp.DAL
{
    public class PustokDbContext : IdentityDbContext<AppUser>
    {
        public PustokDbContext(DbContextOptions<PustokDbContext> options) : base(options) { }

        public DbSet<Slider> Sliders { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BookImage> BookImages { get; set; }
        public DbSet<BasketItem> BasketItems { get; set; }
    }
}
