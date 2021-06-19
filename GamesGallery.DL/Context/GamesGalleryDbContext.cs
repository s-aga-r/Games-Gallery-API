using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GamesGallery.DL.Context
{
    public class GamesGalleryDbContext : IdentityDbContext
    {
        // Public Constructor
        public GamesGalleryDbContext(DbContextOptions<GamesGalleryDbContext> options) : base(options)
        {

        }

        // Entities To Be Added
        public DbSet<Category> Categories { get; set; }
        public DbSet<DownloadLink> DownloadLinks { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Screenshot> Screenshots { get; set; }
        public DbSet<Slider> Sliders { get; set; }

        // Override OnModelCreating Method For Seeding
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Extension Method of ModelBuilder Class To Seed Categories and Games
            //modelBuilder.SeedGames();
        }
    }
}
