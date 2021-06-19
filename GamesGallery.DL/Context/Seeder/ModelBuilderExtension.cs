using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace GamesGallery.DL.Context.Seeder
{
    static class ModelBuilderExtension
    {
        // Seed Categories
        public static void SeedCategories(this ModelBuilder modelBuilder)
        {
            List<Category> categories = new List<Category>();

            for (int i = 1; i < 6; i++)
            {
                Category category = new Category()
                {
                    Id = Guid.NewGuid(),
                    Title = $"Category {i}",
                    Description = "Category {i} Description",
                    Games = null,
                    IsActive = true
                };
                categories.Add(category);
            }

            modelBuilder.Entity<Category>().HasData(categories);
        }

        // Seed Categories + Games
        public static void SeedGames(this ModelBuilder modelBuilder)
        {
            SeedCategories(modelBuilder);

            List<Game> games = new List<Game>();

            for (int i = 1; i < 100; i++)
            {
                Game game = new Game()
                {
                    Id = Guid.NewGuid(),
                    Title = $"Game {i}",
                    Description = $"Game {i} Description",
                    CoverImage = null,
                    Size = 10.1f + i + 2,
                    TotalDownloads = 101 + i * 3,
                    MinimumRequirements = null,
                    RecommendedRequirements = null,
                    VideoTutorial = null,
                    DateOfUpload = DateTime.Now,
                    YearOfRelease = 2021,
                    DownloadLinks = null,
                    Screenshots = null,
                    Categories = null,
                    LastUpdatedOn = DateTime.Now,
                    IsActive = true
                };
                games.Add(game);
            }

            modelBuilder.Entity<Game>().HasData(games);
        }
    }
}
