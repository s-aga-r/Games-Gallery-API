using GamesGallery.DL;
using GamesGallery.DL.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamesGallery.BL
{
    // Private Fields and Public Constructor
    public partial class GamesGalleryRepository : IGamesGalleryRepository
    {
        // Private Fields
        private readonly GamesGalleryDbContext context;

        // Public Constructor
        public GamesGalleryRepository(GamesGalleryDbContext context)
        {
            this.context = context;
        }
    }

    // Public Methods for Games
    public partial class GamesGalleryRepository
    {
        public async Task<List<Game>> GetGamesAsync(int noOfRecords, bool include)
        {
            List<Game> games;

            if (include)
            {
                games = await context.Games.OrderBy(x => x.DateOfUpload).Include(x => x.DownloadLinks).Include(x => x.Screenshots).Include(x => x.Categories).ToListAsync();

                if (games.Count > 0)
                {
                    games.ForEach(x => x.DownloadLinks.ForEach(y => y.Game = null));
                    games.ForEach(x => x.Screenshots.ForEach(y => y.Game = null));
                    games.ForEach(x => x.Categories.ForEach(y => y.Games = null));
                }
            }
            else
            {
                games = await context.Games.OrderBy(x => x.DateOfUpload).ToListAsync();
            }

            if (noOfRecords > 0)
            {
                games = games.Take(noOfRecords).ToList();
            }

            return games;
        }

        public async Task<List<Game>> GetSortedGamesAsync(string sortBy, string orderBy, int noOfRecords, bool include)
        {
            List<Game> games;

            if (include)
            {
                games = await context.Games.Include(x => x.DownloadLinks).Include(x => x.Screenshots).Include(x => x.Categories).ToListAsync();

                if (games.Count > 0)
                {
                    games.ForEach(x => x.DownloadLinks.ForEach(y => y.Game = null));
                    games.ForEach(x => x.Screenshots.ForEach(y => y.Game = null));
                    games.ForEach(x => x.Categories.ForEach(y => y.Games = null));
                }
            }
            else
            {
                games = await context.Games.ToListAsync();
            }

            if (games.Count() > 1)
            {
                if (orderBy == "DESC")
                {
                    switch (sortBy)
                    {
                        case "TITLE":
                            games = games.OrderByDescending(x => x.Title).ToList();
                            break;

                        case "SIZE":
                            games = games.OrderByDescending(x => x.Size).ToList();
                            break;

                        case "TOTALDOWNLOADS":
                            games = games.OrderByDescending(x => x.TotalDownloads).ToList();
                            break;

                        case "YEAROFRELEASE":
                            games = games.OrderByDescending(x => x.YearOfRelease).ToList();
                            break;

                        case "LASTUPDATEDON":
                            games = games.OrderByDescending(x => x.LastUpdatedOn).ToList();
                            break;

                        default:
                            games = games.OrderByDescending(x => x.DateOfUpload).ToList();
                            break;
                    }
                }
                else
                {
                    switch (sortBy)
                    {
                        case "TITLE":
                            games = games.OrderBy(x => x.Title).ToList();
                            break;

                        case "SIZE":
                            games = games.OrderBy(x => x.Size).ToList();
                            break;

                        case "TOTALDOWNLOADS":
                            games = games.OrderBy(x => x.TotalDownloads).ToList();
                            break;

                        case "YEAROFRELEASE":
                            games = games.OrderBy(x => x.YearOfRelease).ToList();
                            break;

                        case "LASTUPDATEDON":
                            games = games.OrderBy(x => x.LastUpdatedOn).ToList();
                            break;

                        default:
                            games = games.OrderBy(x => x.DateOfUpload).ToList();
                            break;
                    }
                }

                if (noOfRecords > 0)
                {
                    games = games.Take(noOfRecords).ToList();
                }
            }

            return games;
        }

        public async Task<List<Game>> GetSearchedGamesAsync(string searchBy, string searchString, int noOfRecords, bool include)
        {
            List<Game> games;

            if (include)
            {
                games = await context.Games.Include(x => x.DownloadLinks).Include(x => x.Screenshots).Include(x => x.Categories).ToListAsync();

                if (games.Count > 0)
                {
                    games.ForEach(x => x.DownloadLinks.ForEach(y => y.Game = null));
                    games.ForEach(x => x.Screenshots.ForEach(y => y.Game = null));
                    games.ForEach(x => x.Categories.ForEach(y => y.Games = null));
                }
            }
            else
            {
                games = await context.Games.ToListAsync();
            }

            if (games.Any())
            {
                switch (searchBy)
                {
                    case "TITLE":
                        games = games.Where(x => x.Title.ToUpper().StartsWith(searchString)).ToList();
                        break;

                    case "SIZE":
                        games = games.Where(x => x.Size.ToString().StartsWith(searchString)).ToList();
                        break;

                    case "TOTALDOWNLOADS":
                        games = games.Where(x => x.TotalDownloads.ToString().StartsWith(searchString)).ToList();
                        break;

                    case "DATEOFUPLOAD":
                        games = games.Where(x => x.DateOfUpload.ToString().StartsWith(searchString)).ToList();
                        break;

                    case "YEAROFRELEASE":
                        games = games.Where(x => x.YearOfRelease.ToString().StartsWith(searchString)).ToList();
                        break;

                    case "LASTUPDATEDON":
                        games = games.Where(x => x.LastUpdatedOn.ToString().StartsWith(searchString)).ToList();
                        break;
                }

                if (noOfRecords > 0)
                {
                    games = games.Take(noOfRecords).ToList();
                }
            }

            return games;
        }

        public async Task<Game> GetGameAsync(Guid id, bool include)
        {
            Game game;

            if (include)
            {
                game = await context.Games.Include(x => x.DownloadLinks).Include(x => x.Screenshots).Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);

                if (game != null)
                {
                    game.DownloadLinks.ForEach(y => y.Game = null);
                    game.Screenshots.ForEach(y => y.Game = null);
                    game.Categories.ForEach(y => y.Games = null);
                }
            }
            else
            {
                game = await context.Games.FirstOrDefaultAsync(x => x.Id == id);
            }

            return game;
        }

        public async Task<string> AddGameAsync(Game game)
        {
            context.Games.Add(game);
            context.Entry(game).State = EntityState.Added;
            int result = await context.SaveChangesAsync();

            return result > 0 ? game.Id.ToString() : null;
        }

        public async Task<string> EditGameAsync(Game game)
        {
            context.Games.Update(game);
            context.Entry(game).State = EntityState.Modified;
            int result = await context.SaveChangesAsync();

            return result > 0 ? game.Id.ToString() : null;
        }

        public async Task<Game> DeleteGameAsync(Guid id)
        {
            Game game = await GetGameAsync(id, true);

            if (game == null)
            {
                return null;
            }
            else
            {
                context.Games.Remove(game);
                context.Entry(game).State = EntityState.Deleted;
                int result = await context.SaveChangesAsync();

                return result > 0 ? game : null;
            }
        }

        // Helper
        public async Task<Game> GetGameIncludeAllAsync(Guid id)
        {
            return await context.Games.Include(x => x.DownloadLinks).Include(x => x.Screenshots).Include(x => x.Categories).FirstOrDefaultAsync(x => x.Id == id);
        }
    }

    // Public Methods for Category
    public partial class GamesGalleryRepository
    {
        public async Task<List<Category>> GetCategoriesAsync(int noOfRecords, bool include)
        {
            List<Category> categories;

            if (include)
            {
                categories = await context.Categories.OrderBy(x => x.Title).Include(x => x.Games).ToListAsync();

                if (categories.Count > 0)
                {
                    categories.ForEach(x => x.Games.ForEach(y => y.DownloadLinks = null));
                    categories.ForEach(x => x.Games.ForEach(y => y.Screenshots = null));
                    categories.ForEach(x => x.Games.ForEach(y => y.Categories = null));
                }
            }
            else
            {
                categories = await context.Categories.OrderBy(x => x.Title).ToListAsync();
            }

            if (noOfRecords > 0)
            {
                categories = categories.Take(noOfRecords).ToList();
            }

            return categories;
        }

        public async Task<List<Category>> GetSearchedCategoriesAsync(string searchString, int noOfRecords, bool include)
        {
            List<Category> categories;

            if (include)
            {
                categories = await context.Categories.Include(x => x.Games).ToListAsync();

                if (categories.Count > 0)
                {
                    categories.ForEach(x => x.Games.ForEach(y => y.DownloadLinks = null));
                    categories.ForEach(x => x.Games.ForEach(y => y.Screenshots = null));
                    categories.ForEach(x => x.Games.ForEach(y => y.Categories = null));
                }
            }
            else
            {
                categories = await context.Categories.ToListAsync();
            }

            if (categories.Any())
            {
                categories = categories.Where(x => x.Title.ToUpper().StartsWith(searchString)).ToList();

                if (noOfRecords > 0)
                {
                    categories = categories.Take(noOfRecords).ToList();
                }
            }

            return categories;
        }

        public async Task<Category> GetCategoryAsync(Guid id, bool include)
        {
            Category category;

            if (include)
            {
                category = await context.Categories.Include(x => x.Games).FirstOrDefaultAsync(x => x.Id == id);

                if (category != null)
                {
                    category.Games.ForEach(x => x.DownloadLinks = null);
                    category.Games.ForEach(x => x.Screenshots = null);
                    category.Games.ForEach(x => x.Categories = null);
                }
            }
            else
            {
                category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            }

            return category;
        }

        public async Task<string> AddCategoryAsync(Category category)
        {
            context.Categories.Add(category);
            context.Entry(category).State = EntityState.Added;
            int result = await context.SaveChangesAsync();

            return result > 0 ? category.Id.ToString() : null;
        }

        public async Task<string> EditCategoryAsync(Category category)
        {
            context.Categories.Update(category);
            context.Entry(category).State = EntityState.Modified;
            int result = await context.SaveChangesAsync();

            return result > 0 ? category.Id.ToString() : null;
        }

        public async Task<Category> DeleteCategoryAsync(Guid id)
        {
            Category category = await GetCategoryAsync(id, true);

            if (category == null)
            {
                return null;
            }
            else
            {
                context.Categories.Remove(category);
                context.Entry(category).State = EntityState.Deleted;
                int result = await context.SaveChangesAsync();

                return result > 0 ? category : null;
            }
        }
    }

    // Public Methods for DownloadLink
    public partial class GamesGalleryRepository
    {
        public async Task<List<DownloadLink>> GetDownloadLinksAsync(int noOfRecords, bool include)
        {
            List<DownloadLink> downloadLinks;

            if (include)
            {
                downloadLinks = await context.DownloadLinks.OrderBy(x => x.Title).Include(x => x.Game).ToListAsync();

                if (downloadLinks.Count > 0)
                {
                    downloadLinks.ForEach(x => x.Game.DownloadLinks = null);
                    downloadLinks.ForEach(x => x.Game.Screenshots = null);
                    downloadLinks.ForEach(x => x.Game.Categories = null);
                }
            }
            else
            {
                downloadLinks = await context.DownloadLinks.OrderBy(x => x.Title).ToListAsync();
            }

            if (noOfRecords > 0)
            {
                downloadLinks = downloadLinks.Take(noOfRecords).ToList();
            }

            return downloadLinks;
        }

        public async Task<List<DownloadLink>> GetSearchedDownloadLinksAsync(string searchBy, string searchString, int noOfRecords, bool include)
        {
            List<DownloadLink> downloadLinks;

            if (include)
            {
                downloadLinks = await context.DownloadLinks.Include(x => x.Game).ToListAsync();

                if (downloadLinks.Count > 0)
                {
                    downloadLinks.ForEach(x => x.Game.DownloadLinks = null);
                    downloadLinks.ForEach(x => x.Game.Screenshots = null);
                    downloadLinks.ForEach(x => x.Game.Categories = null);
                }
            }
            else
            {
                downloadLinks = await context.DownloadLinks.ToListAsync();
            }

            if (downloadLinks.Any())
            {
                switch (searchBy)
                {
                    case "TITLE":
                        downloadLinks = downloadLinks.Where(x => x.Title != null && x.Title.ToUpper().StartsWith(searchString)).ToList();
                        break;

                    case "LINK":
                        downloadLinks = downloadLinks.Where(x => x.Link.ToUpper().StartsWith(searchString)).ToList();
                        break;
                }

                if (noOfRecords > 0)
                {
                    downloadLinks = downloadLinks.Take(noOfRecords).ToList();
                }
            }

            return downloadLinks;
        }

        public async Task<DownloadLink> GetDownloadLinkAsync(Guid id, bool include)
        {
            DownloadLink downloadLink;

            if (include)
            {
                downloadLink = await context.DownloadLinks.Include(x => x.Game).FirstOrDefaultAsync(x => x.Id == id);

                if (downloadLink != null)
                {
                    downloadLink.Game.DownloadLinks = null;
                    downloadLink.Game.Screenshots = null;
                    downloadLink.Game.Categories = null;
                }
            }
            else
            {
                downloadLink = await context.DownloadLinks.FirstOrDefaultAsync(x => x.Id == id);
            }

            return downloadLink;
        }

        public async Task<string> AddDownloadLinkAsync(DownloadLink downloadLink)
        {
            context.DownloadLinks.Add(downloadLink);
            context.Entry(downloadLink).State = EntityState.Added;
            int result = await context.SaveChangesAsync();

            return result > 0 ? downloadLink.Id.ToString() : null;
        }

        public async Task<string> EditDownloadLinkAsync(DownloadLink downloadLink)
        {
            context.DownloadLinks.Update(downloadLink);
            context.Entry(downloadLink).State = EntityState.Modified;
            int result = await context.SaveChangesAsync();

            return result > 0 ? downloadLink.Id.ToString() : null;
        }

        public async Task<DownloadLink> DeleteDownloadLinkAsync(Guid id)
        {
            DownloadLink downloadLink = await GetDownloadLinkAsync(id, true);

            if (downloadLink == null)
            {
                return null;
            }
            else
            {
                context.DownloadLinks.Remove(downloadLink);
                context.Entry(downloadLink).State = EntityState.Deleted;
                int result = await context.SaveChangesAsync();

                return result > 0 ? downloadLink : null;
            }
        }
    }

    // Public Methods for Screenshot
    public partial class GamesGalleryRepository
    {
        public async Task<List<Screenshot>> GetScreenshotsAsync(int noOfRecords, bool include)
        {
            List<Screenshot> screenshots;

            if (include)
            {
                screenshots = await context.Screenshots.Include(x => x.Game).ToListAsync();

                if (screenshots.Count > 0)
                {
                    screenshots.ForEach(x => x.Game.DownloadLinks = new List<DownloadLink>());
                    screenshots.ForEach(x => x.Game.Screenshots = new List<Screenshot>());
                    screenshots.ForEach(x => x.Game.Categories = new List<Category>());
                }
            }
            else
            {
                screenshots = await context.Screenshots.ToListAsync();
            }

            if (noOfRecords > 0)
            {
                screenshots = screenshots.Take(noOfRecords).ToList();
            }

            return screenshots;
        }

        public async Task<Screenshot> GetScreenshotAsync(Guid id, bool include)
        {
            Screenshot screenshot;

            if (include)
            {
                screenshot = await context.Screenshots.Include(x => x.Game).FirstOrDefaultAsync(x => x.Id == id);

                if (screenshot != null)
                {
                    screenshot.Game.DownloadLinks.ForEach(y => y.Game = null);
                    screenshot.Game.Screenshots.ForEach(y => y.Game = null);
                    screenshot.Game.Categories.ForEach(y => y.Games = null);
                }
            }
            else
            {
                screenshot = await context.Screenshots.FirstOrDefaultAsync(x => x.Id == id);
            }

            return screenshot;
        }

        public async Task<string> AddScreenshotAsync(Screenshot screenshot)
        {
            context.Screenshots.Add(screenshot);
            context.Entry(screenshot).State = EntityState.Added;
            int result = await context.SaveChangesAsync();

            return result > 0 ? screenshot.Id.ToString() : null;
        }

        public async Task<string> EditScreenshotAsync(Screenshot screenshot)
        {
            context.Screenshots.Update(screenshot);
            context.Entry(screenshot).State = EntityState.Modified;
            int result = await context.SaveChangesAsync();

            return result > 0 ? screenshot.Id.ToString() : null;
        }

        public async Task<Screenshot> DeleteScreenshotAsync(Guid id)
        {
            Screenshot screenshot = await GetScreenshotAsync(id, true);

            if (screenshot == null)
            {
                return null;
            }
            else
            {
                context.Screenshots.Remove(screenshot);
                context.Entry(screenshot).State = EntityState.Deleted;
                int result = await context.SaveChangesAsync();

                return result > 0 ? screenshot : null;
            }
        }
    }

    // Public Methods for Slider
    public partial class GamesGalleryRepository
    {
        public async Task<List<Slider>> GetSlidersAsync(int noOfRecords, bool include)
        {
            List<Slider> sliders;

            if (include)
            {
                sliders = await context.Sliders.OrderBy(x => x.Order).Include(x => x.Game).ToListAsync();

                if (sliders.Count > 0)
                {
                    sliders.ForEach(x => x.Game.DownloadLinks.ForEach(y => y.Game = null));
                    sliders.ForEach(x => x.Game.Screenshots.ForEach(y => y.Game = null));
                    sliders.ForEach(x => x.Game.Categories.ForEach(y => y.Games = null));
                }
            }
            else
            {
                sliders = await context.Sliders.OrderBy(x => x.Order).ToListAsync();
            }

            if (noOfRecords > 0)
            {
                sliders = sliders.Take(noOfRecords).ToList();
            }

            return sliders;
        }

        public async Task<Slider> GetSliderAsync(Guid id, bool include)
        {
            Slider slider;

            if (include)
            {
                slider = await context.Sliders.Include(x => x.Game).FirstOrDefaultAsync(x => x.Id == id);

                if (slider != null)
                {
                    slider.Game.DownloadLinks.ForEach(y => y.Game = null);
                    slider.Game.Screenshots.ForEach(y => y.Game = null);
                    slider.Game.Categories.ForEach(y => y.Games = null);
                }
            }
            else
            {
                slider = await context.Sliders.FirstOrDefaultAsync(x => x.Id == id);
            }

            return slider;
        }

        public async Task<string> AddSliderAsync(Slider slider)
        {
            context.Sliders.Add(slider);
            context.Entry(slider).State = EntityState.Added;
            int result = await context.SaveChangesAsync();

            return result > 0 ? slider.Id.ToString() : null;
        }

        public async Task<string> EditSliderAsync(Slider slider)
        {
            context.Sliders.Update(slider);
            context.Entry(slider).State = EntityState.Modified;
            int result = await context.SaveChangesAsync();

            return result > 0 ? slider.Id.ToString() : null;
        }

        public async Task<Slider> DeleteSliderAsync(Guid id)
        {
            Slider slider = await GetSliderAsync(id, true);

            if (slider == null)
            {
                return null;
            }
            else
            {
                context.Sliders.Remove(slider);
                context.Entry(slider).State = EntityState.Deleted;
                int result = await context.SaveChangesAsync();

                return result > 0 ? slider : null;
            }
        }
    }
}