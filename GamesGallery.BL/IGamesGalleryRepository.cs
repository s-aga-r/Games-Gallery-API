using GamesGallery.DL;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GamesGallery.BL
{
    public interface IGamesGalleryRepository
    {
        Task<string> AddCategoryAsync(Category category);
        Task<string> AddDownloadLinkAsync(DownloadLink downloadLink);
        Task<string> AddGameAsync(Game game);
        Task<string> AddScreenshotAsync(Screenshot screenshot);
        Task<string> AddSliderAsync(Slider slider);
        Task<Category> DeleteCategoryAsync(Guid id);
        Task<DownloadLink> DeleteDownloadLinkAsync(Guid id);
        Task<Game> DeleteGameAsync(Guid id);
        Task<Screenshot> DeleteScreenshotAsync(Guid id);
        Task<Slider> DeleteSliderAsync(Guid id);
        Task<string> EditCategoryAsync(Category category);
        Task<string> EditDownloadLinkAsync(DownloadLink downloadLink);
        Task<string> EditGameAsync(Game game);
        Task<string> EditScreenshotAsync(Screenshot screenshot);
        Task<string> EditSliderAsync(Slider slider);
        Task<List<Category>> GetCategoriesAsync(int noOfRecords, bool include);
        Task<Category> GetCategoryAsync(Guid id, bool include);
        Task<DownloadLink> GetDownloadLinkAsync(Guid id, bool include);
        Task<List<DownloadLink>> GetDownloadLinksAsync(int noOfRecords, bool include);
        Task<Game> GetGameAsync(Guid id, bool include);
        Task<Game> GetGameIncludeAllAsync(Guid id);
        Task<List<Game>> GetGamesAsync(int noOfRecords, bool include);
        Task<Screenshot> GetScreenshotAsync(Guid id, bool include);
        Task<List<Screenshot>> GetScreenshotsAsync(int noOfRecords, bool include);
        Task<List<Category>> GetSearchedCategoriesAsync(string searchString, int noOfRecords, bool include);
        Task<List<DownloadLink>> GetSearchedDownloadLinksAsync(string searchBy, string searchString, int noOfRecords, bool include);
        Task<List<Game>> GetSearchedGamesAsync(string searchBy, string searchString, int noOfRecords, bool include);
        Task<Slider> GetSliderAsync(Guid id, bool include);
        Task<List<Slider>> GetSlidersAsync(int noOfRecords, bool include);
        Task<List<Game>> GetSortedGamesAsync(string sortBy, string orderBy, int noOfRecords, bool include);
    }
}