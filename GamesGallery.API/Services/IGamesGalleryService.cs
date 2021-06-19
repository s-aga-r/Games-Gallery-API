using GamesGallery.VM;
using GamesGallery.VM.CreateVM;
using GamesGallery.VM.EditVM;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GamesGallery.API.Services
{
    public interface IGamesGalleryService
    {
        Task<string> AddCategoryAsync(CreateCategoryVM model);
        Task<string> AddDownloadLinkAsync(CreateDownloadLinkVM model);
        Task<string> AddGameAsync(CreateGameVM model);
        Task<string> AddScreenshotAsync(CreateScreenshotVM model);
        Task<string> AddSliderAsync(CreateSliderVM model);
        Task<string> DeleteCategoryAsync(Guid id);
        Task<string> DeleteDownloadLinkAsync(Guid id);
        Task<string> DeleteGameAsync(Guid id);
        Task<string> DeleteScreenshotAsync(Guid id);
        Task<string> DeleteSliderAsync(Guid id);
        Task<string> EditCategoryAsync(EditCategoryVM model);
        Task<string> EditDownloadLinkAsync(EditDownloadLinkVM model);
        Task<string> EditGameAsync(EditGameVM model);
        Task<string> EditScreenshotAsync(EditScreenshotVM model);
        Task<string> EditSliderAsync(EditSliderVM model);
        Task<List<CategoryVM>> GetCategoriesAsync(int noOfRecords, bool include);
        Task<CategoryVM> GetCategoryAsync(Guid id, bool include);
        Task<DownloadLinkVM> GetDownloadLinkAsync(Guid id, bool include);
        Task<List<DownloadLinkVM>> GetDownloadLinksAsync(int noOfRecords, bool include);
        Task<GameVM> GetGameAsync(Guid id, bool include);
        Task<List<GameVM>> GetGamesAsync(int noOfRecords, bool include);
        FileStream GetImage(string fileName);
        Task<ScreenshotVM> GetScreenshotAsync(Guid id, bool include);
        Task<List<ScreenshotVM>> GetScreenshotsAsync(int noOfRecords, bool include);
        Task<List<CategoryVM>> GetSearchedCategoriesAsync(string searchString, int noOfRecords, bool include);
        Task<List<DownloadLinkVM>> GetSearchedDownloadLinksAsync(string searchBy, string searchString, int noOfRecords, bool include);
        Task<List<GameVM>> GetSearchedGamesAsync(string searchBy, string searchString, int noOfRecords, bool include);
        Task<SliderVM> GetSliderAsync(Guid id, bool include);
        Task<List<SliderVM>> GetSlidersAsync(int noOfRecords, bool include);
        Task<List<GameVM>> GetSortedGamesAsync(string sortBy, string orderBy, int noOfRecords, bool include);
    }
}