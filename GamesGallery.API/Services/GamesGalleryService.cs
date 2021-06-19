using AutoMapper;
using GamesGallery.BL;
using GamesGallery.DL;
using GamesGallery.VM;
using GamesGallery.VM.CreateVM;
using GamesGallery.VM.EditVM;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GamesGallery.API.Services
{
    // Private Fields and Public Constructor
    public partial class GamesGalleryService : IGamesGalleryService
    {
        // Private Fields
        private readonly IGamesGalleryRepository repository;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IMapper mapper;
        private readonly string ImageUploadFolder;
        private readonly string AllowedImageExtensions;
        private readonly long MinimumImageSize;
        private readonly long MaximumImageSize;
        private readonly bool IsUploadFolderAvailable;

        // Public Constructor
        public GamesGalleryService(IGamesGalleryRepository repository, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IMapper mapper)
        {
            this.repository = repository;
            this.configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
            this.mapper = mapper;
            this.ImageUploadFolder = Path.Combine(webHostEnvironment.ContentRootPath, configuration["GamesGalleryConfigurations:ImageConfigurations:SaveTo"] ?? "Images");
            this.AllowedImageExtensions = configuration.GetValue<string>("GamesGalleryConfigurations:ImageConfigurations:AllowedExtensions") ?? ".png, .jpg, .jpeg";
            this.MinimumImageSize = configuration.GetValue<long>("GamesGalleryConfigurations:ImageConfigurations:MinimumSizeInBytes") > 999 ? configuration.GetValue<long>("GamesGalleryConfigurations:ImageConfigurations:MinimumSizeInBytes") : 999;
            this.MaximumImageSize = configuration.GetValue<long>("GamesGalleryConfigurations:ImageConfigurations:MaximumSizeInBytes") < 10000000 ? configuration.GetValue<long>("GamesGalleryConfigurations:ImageConfigurations:MaximumSizeInBytes") : 10000000;

            if (!Directory.Exists(ImageUploadFolder))
            {
                Directory.CreateDirectory(ImageUploadFolder);
            }

            IsUploadFolderAvailable = true;
        }
    }

    // Public Methods for Games
    public partial class GamesGalleryService
    {
        public async Task<List<GameVM>> GetGamesAsync(int noOfRecords, bool include)
        {
            List<Game> games = await repository.GetGamesAsync(noOfRecords, include);

            if (games == null)
            {
                return null;
            }
            else
            {
                List<GameVM> gamesVM = mapper.Map<List<GameVM>>(games);

                return gamesVM;
            }
        }

        public async Task<List<GameVM>> GetSortedGamesAsync(string sortBy, string orderBy, int noOfRecords, bool include)
        {
            List<Game> games = await repository.GetSortedGamesAsync(sortBy, orderBy, noOfRecords, include);

            if (games == null)
            {
                return null;
            }
            else
            {
                List<GameVM> gamesVM = mapper.Map<List<GameVM>>(games);

                return gamesVM;
            }
        }

        public async Task<List<GameVM>> GetSearchedGamesAsync(string searchBy, string searchString, int noOfRecords, bool include)
        {
            List<Game> games = await repository.GetSearchedGamesAsync(searchBy, searchString, noOfRecords, include);

            if (games == null)
            {
                return null;
            }
            else
            {
                List<GameVM> gamesVM = mapper.Map<List<GameVM>>(games);

                return gamesVM;
            }
        }

        public async Task<GameVM> GetGameAsync(Guid id, bool include)
        {
            Game game = await repository.GetGameAsync(id, include);

            if (game == null)
            {
                return null;
            }
            else
            {
                GameVM gameVM = mapper.Map<GameVM>(game);

                return gameVM;
            }
        }

        public async Task<string> AddGameAsync(CreateGameVM model)
        {
            Game game = mapper.Map<Game>(model);
            game.CoverImage = await SaveImageAsync(model.CoverImageIFormFile);

            if (game.CoverImage != null)
            {
                game.DownloadLinks = DownloadLinksStringToDownloadLinkVM(model.DownloadLinksString, model.Title);

                if (game.DownloadLinks != null)
                {
                    game.Screenshots = await SaveScreenshotsAsync(model.ScreenshotsIFormFile);
                    game.Categories = await CategoriesIdToCategoryListAsync(model.CategoriesId);

                    if (game.Categories != null && game.Categories.Count() > 0)
                    {
                        game.DateOfUpload = DateTime.Now;
                        game.LastUpdatedOn = DateTime.Now;

                        return await repository.AddGameAsync(game);
                    }
                    else
                    {
                        return "Category/Categories in which you are trying to add the game were not found.";
                    }
                }
                else
                {
                    return "Provide valid Download Link(s) to add a game.";
                }
            }
            else
            {
                return $"Cover Image is required with an minimum size of {((MinimumImageSize) / 1000)} KB and maximum size of {((MaximumImageSize) / 1000) / 1000} MB in {AllowedImageExtensions} extension.";
            }
        }

        public async Task<string> EditGameAsync(EditGameVM model)
        {
            Game game = await repository.GetGameIncludeAllAsync(model.Id);

            if (game != null)
            {
                string CoverImageOld = game.CoverImage;
                List<Screenshot> ScreenshotsOld = game.Screenshots;
                string CoverImageNew = await SaveImageAsync(model.CoverImageIFormFile);
                List<Screenshot> ScreenshotsNew = null;

                if (model.CoverImageIFormFile != null)
                {
                    if (CoverImageNew != null)
                    {
                        game.CoverImage = CoverImageNew;
                    }
                    else
                    {
                        return $"If you want to change the Cover Image then provide a image with a minimum size of {((MinimumImageSize) / 1000)} KB and maximum size of {((MaximumImageSize) / 1000) / 1000} MB in {AllowedImageExtensions} extension.";
                    }
                }

                List<DownloadLink> DownloadLinks = DownloadLinksStringToDownloadLinkVM(model.DownloadLinksString, model.Title ?? game.Title);
                if (!string.IsNullOrEmpty(model.DownloadLinksString))
                {
                    if (DownloadLinks != null)
                    {
                        game.DownloadLinks = DownloadLinks;
                    }
                    else
                    {
                        return "If you want to change Download Link(s) then provide valid Download Link(s).";
                    }
                }

                if (model.ScreenshotsIFormFile != null && model.ScreenshotsIFormFile.Count() > 0)
                {
                    ScreenshotsNew = await SaveScreenshotsAsync(model.ScreenshotsIFormFile);

                    if (ScreenshotsNew != null)
                    {
                        game.Screenshots = ScreenshotsNew;
                    }
                    else
                    {
                        return $"One or more Screenshot(s) are not matching with the allowed Screenshot Properties, If you want to change Screenshot(s) then provide Screenshot(s) with a minimum size of {((MinimumImageSize) / 1000)} KB and maximum size of {((MaximumImageSize) / 1000) / 1000} MB in {AllowedImageExtensions} extension.";
                    }
                }

                List<Category> Categories = await CategoriesIdToCategoryListAsync(model.CategoriesId);
                if (model.CategoriesId != null && model.CategoriesId.Count() > 0)
                {
                    if (Categories != null)
                    {
                        game.Categories = Categories;
                    }
                    else
                    {
                        return "If you want to change Categories of the Game then you have to provide valid Categories Id.";
                    }
                }

                game.Title = model.Title ?? game.Title;
                game.Description = model.Description ?? game.Description;
                game.Size = model.Size ?? game.Size;
                game.MinimumRequirements = model.MinimumRequirements ?? game.MinimumRequirements;
                game.RecommendedRequirements = model.RecommendedRequirements ?? game.RecommendedRequirements;
                game.VideoTutorial = model.VideoTutorial ?? game.RecommendedRequirements;
                game.YearOfRelease = model.YearOfRelease ?? game.YearOfRelease;
                game.LastUpdatedOn = DateTime.Now;
                game.IsActive = model.IsActive ?? game.IsActive;

                string updatedGameId = await repository.EditGameAsync(game);

                if (updatedGameId == null)
                {
                    if (CoverImageNew != null)
                    {
                        DeleteImage(CoverImageNew);
                    }
                    if (ScreenshotsNew != null)
                    {
                        DeleteScreenshots(ScreenshotsNew);
                    }
                }
                else
                {
                    if (CoverImageNew != null)
                    {
                        DeleteImage(CoverImageOld);
                    }
                    if (ScreenshotsNew != null)
                    {
                        DeleteScreenshots(ScreenshotsOld);
                    }
                }

                return updatedGameId;
            }
            else
            {
                return "";
            }
        }

        public async Task<string> DeleteGameAsync(Guid id)
        {
            Game game = await repository.DeleteGameAsync(id);

            if (game == null)
            {
                return null;
            }
            else
            {
                DeleteImage(game.CoverImage);
                DeleteScreenshots(game.Screenshots);

                return game.Id.ToString();
            }
        }
    }

    // Public Methods for Category
    public partial class GamesGalleryService
    {
        public async Task<List<CategoryVM>> GetCategoriesAsync(int noOfRecords, bool include)
        {
            List<Category> categories = await repository.GetCategoriesAsync(noOfRecords, include);

            if (categories == null)
            {
                return null;
            }
            else
            {
                List<CategoryVM> categoriesVM = mapper.Map<List<CategoryVM>>(categories);

                return categoriesVM;
            }
        }

        public async Task<List<CategoryVM>> GetSearchedCategoriesAsync(string searchString, int noOfRecords, bool include)
        {
            List<Category> categories = await repository.GetSearchedCategoriesAsync(searchString, noOfRecords, include);

            if (categories == null)
            {
                return null;
            }
            else
            {
                List<CategoryVM> categoryVM = mapper.Map<List<CategoryVM>>(categories);

                return categoryVM;
            }
        }

        public async Task<CategoryVM> GetCategoryAsync(Guid id, bool include)
        {
            Category category = await repository.GetCategoryAsync(id, include);

            if (category == null)
            {
                return null;
            }
            else
            {
                CategoryVM categoryVM = mapper.Map<CategoryVM>(category);

                return categoryVM;
            }
        }

        public async Task<string> AddCategoryAsync(CreateCategoryVM model)
        {
            List<Category> categories = await repository.GetSearchedCategoriesAsync(model.Title.ToUpper(), 1, false);

            if(categories != null && categories.Count() > 0)
            {
                Category tempCategory = categories.FirstOrDefault();

                if(tempCategory.IsActive == true)
                {
                    return "The category you are trying to add is already in the list.";
                }
                else
                {
                    return "The category you are trying to add is already in the list with active status false.";
                }
            }
            else
            {
                Category category = mapper.Map<Category>(model);

                return await repository.AddCategoryAsync(category);
            }

            //if (model.GamesId != null && model.GamesId.Count() > 0)
            //{
            //    List<Game> games = await GamesIdToGamesListAsync(model.GamesId);

            //    if (games != null)
            //    {
            //        category.Games = games;
            //    }
            //    else
            //    {
            //        return "One or more invalid Game(s) Id found.";
            //    }
            //} 
        }

        public async Task<string> EditCategoryAsync(EditCategoryVM model)
        {
            Category category = await repository.GetCategoryAsync(model.Id, true);

            if (category != null)
            {
                //List<Game> games = await GamesIdToGamesListAsync(model.GamesId);

                //if (model.GamesId != null && model.GamesId.Count() > 0)
                //{
                //    if (games != null)
                //    {
                //        category.Games = games;
                //    }
                //    else
                //    {
                //        return "If you want to change Games of the Category then you have to provide valid Game(s) Id.";
                //    }
                //}

                category.Title = model.Title;
                category.Description = model.Description;
                category.IsActive = model.IsActive;

                return await repository.EditCategoryAsync(category);
            }
            else
            {
                return "";
            }
        }

        public async Task<string> DeleteCategoryAsync(Guid id)
        {
            Category category = await repository.DeleteCategoryAsync(id);

            if (category == null)
            {
                return null;
            }
            else
            {
                return category.Id.ToString();
            }
        }
    }

    // Public Methods for DownloadLink
    public partial class GamesGalleryService
    {
        public async Task<List<DownloadLinkVM>> GetDownloadLinksAsync(int noOfRecords, bool include)
        {
            List<DownloadLink> downloadLinks = await repository.GetDownloadLinksAsync(noOfRecords, include);

            if (downloadLinks == null)
            {
                return null;
            }
            else
            {
                List<DownloadLinkVM> downloadLinkVM = mapper.Map<List<DownloadLinkVM>>(downloadLinks);

                return downloadLinkVM;
            }
        }

        public async Task<List<DownloadLinkVM>> GetSearchedDownloadLinksAsync(string searchBy, string searchString, int noOfRecords, bool include)
        {
            List<DownloadLink> downloadLinks = await repository.GetSearchedDownloadLinksAsync(searchBy, searchString, noOfRecords, include);

            if (downloadLinks == null)
            {
                return null;
            }
            else
            {
                List<DownloadLinkVM> downloadLinkVM = mapper.Map<List<DownloadLinkVM>>(downloadLinks);

                return downloadLinkVM;
            }
        }

        public async Task<DownloadLinkVM> GetDownloadLinkAsync(Guid id, bool include)
        {
            DownloadLink downloadLink = await repository.GetDownloadLinkAsync(id, include);

            if (downloadLink == null)
            {
                return null;
            }
            else
            {
                DownloadLinkVM downloadLinkVM = mapper.Map<DownloadLinkVM>(downloadLink);

                return downloadLinkVM;
            }
        }

        public async Task<string> AddDownloadLinkAsync(CreateDownloadLinkVM model)
        {
            DownloadLink downloadLink = mapper.Map<DownloadLink>(model);

            Game game = await repository.GetGameAsync(model.GameId, false);

            if (game != null)
            {
                downloadLink.Title = downloadLink.Title ?? game.Title;
                downloadLink.GameId = game.Id;
            }
            else
            {
                return "Provide a valid Game Id to add a Download link.";
            }

            return await repository.AddDownloadLinkAsync(downloadLink);
        }

        public async Task<string> EditDownloadLinkAsync(EditDownloadLinkVM model)
        {
            DownloadLink downloadLink = await repository.GetDownloadLinkAsync(model.Id, false);

            if (downloadLink != null)
            {
                if (model.GameId != null)
                {
                    Game game = await repository.GetGameAsync(model.GameId.Value, false);

                    if (game != null)
                    {
                        downloadLink.GameId = game.Id;
                    }
                    else
                    {
                        return "Provide a valid Game Id to update a Download link.";
                    }
                }

                downloadLink.Title = model.Title ?? downloadLink.Title;
                downloadLink.Link = model.Link ?? downloadLink.Link;
                downloadLink.IsActive = model.IsActive ?? downloadLink.IsActive;

                return await repository.EditDownloadLinkAsync(downloadLink);
            }
            else
            {
                return "";
            }
        }

        public async Task<string> DeleteDownloadLinkAsync(Guid id)
        {
            DownloadLink downloadLink = await repository.DeleteDownloadLinkAsync(id);

            if (downloadLink == null)
            {
                return null;
            }
            else
            {
                return downloadLink.Id.ToString();
            }
        }
    }

    // Public Methods for Screenshots
    public partial class GamesGalleryService
    {
        public async Task<List<ScreenshotVM>> GetScreenshotsAsync(int noOfRecords, bool include)
        {
            List<Screenshot> screenshots = await repository.GetScreenshotsAsync(noOfRecords, include);

            if (screenshots == null)
            {
                return null;
            }
            else
            {
                List<ScreenshotVM> screenshotVM = mapper.Map<List<ScreenshotVM>>(screenshots);

                return screenshotVM;
            }
        }

        public async Task<ScreenshotVM> GetScreenshotAsync(Guid id, bool include)
        {
            Screenshot screenshot = await repository.GetScreenshotAsync(id, include);

            if (screenshot == null)
            {
                return null;
            }
            else
            {
                ScreenshotVM screenshotVM = mapper.Map<ScreenshotVM>(screenshot);

                return screenshotVM;
            }
        }

        public async Task<string> AddScreenshotAsync(CreateScreenshotVM model)
        {
            Screenshot screenshot = mapper.Map<Screenshot>(model);

            screenshot.Image = await SaveImageAsync(model.ImageIFormFile);

            if (screenshot.Image != null)
            {
                Game game = await repository.GetGameAsync(model.GameId, false);

                if (game != null)
                {
                    screenshot.Title = screenshot.Title ?? game.Title;
                    screenshot.GameId = game.Id;
                }
                else
                {
                    return "Provide a valid Game Id to add a Screenshot.";
                }

                return await repository.AddScreenshotAsync(screenshot);
            }
            else
            {
                return $"Image is required with an minimum size of {((MinimumImageSize) / 1000)} KB and maximum size of {((MaximumImageSize) / 1000) / 1000} MB in {AllowedImageExtensions} extension.";
            }
        }

        public async Task<string> EditScreenshotAsync(EditScreenshotVM model)
        {
            Screenshot screenshot = await repository.GetScreenshotAsync(model.Id, false);

            if (screenshot != null)
            {
                if (model.GameId != null)
                {
                    Game game = await repository.GetGameAsync(model.GameId.Value, false);

                    if (game != null)
                    {
                        screenshot.GameId = game.Id;
                    }
                    else
                    {
                        return "Provide a valid Game Id to update a Screenshot.";
                    }
                }

                string imageOld = screenshot.Image;
                string imageNew = await SaveImageAsync(model.ImageIFormFile);

                if (model.ImageIFormFile != null)
                {
                    if (imageNew != null)
                    {
                        screenshot.Image = imageNew;
                    }
                    else
                    {
                        return $"If you want to change the Screenshot Image then provide a image with a minimum size of {((MinimumImageSize) / 1000)} KB and maximum size of {((MaximumImageSize) / 1000) / 1000} MB in {AllowedImageExtensions} extension.";
                    }
                }

                screenshot.Title = model.Title ?? screenshot.Title;
                screenshot.Description = model.Description ?? screenshot.Description;
                screenshot.IsActive = model.IsActive ?? screenshot.IsActive;

                string updatedScreenshotId = await repository.EditScreenshotAsync(screenshot);

                if (updatedScreenshotId == null && imageNew != null)
                {
                    DeleteImage(imageNew);
                }
                else if (updatedScreenshotId != null && imageNew != null)
                {
                    DeleteImage(imageOld);
                }

                return updatedScreenshotId;
            }
            else
            {
                return "";
            }
        }

        public async Task<string> DeleteScreenshotAsync(Guid id)
        {
            Screenshot screenshot = await repository.DeleteScreenshotAsync(id);

            if (screenshot == null)
            {
                return null;
            }
            else
            {
                DeleteImage(screenshot.Image);

                return screenshot.Id.ToString();
            }
        }
    }

    // Public Methods for Sliders
    public partial class GamesGalleryService
    {
        public async Task<List<SliderVM>> GetSlidersAsync(int noOfRecords, bool include)
        {
            List<Slider> sliders = await repository.GetSlidersAsync(noOfRecords, include);

            if (sliders == null)
            {
                return null;
            }
            else
            {
                List<SliderVM> sliderVM = mapper.Map<List<SliderVM>>(sliders);

                return sliderVM;
            }
        }

        public async Task<SliderVM> GetSliderAsync(Guid id, bool include)
        {
            Slider slider = await repository.GetSliderAsync(id, include);

            if (slider == null)
            {
                return null;
            }
            else
            {
                SliderVM sliderVM = mapper.Map<SliderVM>(slider);

                return sliderVM;
            }
        }

        public async Task<string> AddSliderAsync(CreateSliderVM model)
        {
            Slider slider = mapper.Map<Slider>(model);

            slider.Image = await SaveImageAsync(model.ImageIFormFile);

            if (slider.Image != null)
            {
                Game game = await repository.GetGameAsync(model.GameId, false);

                if (game != null)
                {
                    slider.GameId = game.Id;
                }
                else
                {
                    return "Provide a valid Game Id to add a Slider.";
                }

                slider.DateOfUpload = DateTime.Now;
                slider.LastUpdatedOn = DateTime.Now;

                return await repository.AddSliderAsync(slider);
            }
            else
            {
                return $"Image is required with an minimum size of {((MinimumImageSize) / 1000)} KB and maximum size of {((MaximumImageSize) / 1000) / 1000} MB in {AllowedImageExtensions} extension.";
            }
        }

        public async Task<string> EditSliderAsync(EditSliderVM model)
        {
            Slider slider = await repository.GetSliderAsync(model.Id, false);

            if (slider != null)
            {
                if (model.GameId != null)
                {
                    Game game = await repository.GetGameAsync(model.GameId.Value, false);

                    if (game != null)
                    {
                        slider.GameId = game.Id;
                    }
                    else
                    {
                        return "Provide a valid Game Id to update a Slider.";
                    }
                }

                string imageOld = slider.Image;
                string imageNew = await SaveImageAsync(model.ImageIFormFile);

                if (model.ImageIFormFile != null)
                {
                    if (imageNew != null)
                    {
                        slider.Image = imageNew;
                    }
                    else
                    {
                        return $"If you want to change the Slider Image then provide a image with a minimum size of {((MinimumImageSize) / 1000)} KB and maximum size of {((MaximumImageSize) / 1000) / 1000} MB in {AllowedImageExtensions} extension.";
                    }
                }

                slider.Order = model.Order ?? slider.Order;
                slider.IsActive = model.IsActive ?? slider.IsActive;
                slider.LastUpdatedOn = DateTime.Now;

                string updatedScreenshotId = await repository.EditSliderAsync(slider);

                if (updatedScreenshotId == null && imageNew != null)
                {
                    DeleteImage(imageNew);
                }
                else if (updatedScreenshotId != null && imageNew != null)
                {
                    DeleteImage(imageOld);
                }

                return updatedScreenshotId;
            }
            else
            {
                return "";
            }
        }

        public async Task<string> DeleteSliderAsync(Guid id)
        {
            Slider slider = await repository.DeleteSliderAsync(id);

            if (slider == null)
            {
                return null;
            }
            else
            {
                DeleteImage(slider.Image);

                return slider.Id.ToString();
            }
        }
    }

    // Other Public Methods
    public partial class GamesGalleryService
    {
        public FileStream GetImage(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName);

            if (AllowedImageExtensions.Contains(fileExtension) && IsUploadFolderAvailable)
            {
                string filePath = Path.Combine(ImageUploadFolder, fileName);

                if (File.Exists(filePath))
                {
                    return File.OpenRead(filePath);
                }
            }

            return null;
        }
    }

    // Private Helper Methods
    public partial class GamesGalleryService
    {
        private async Task<string> SaveImageAsync(IFormFile image)
        {
            if (image != null && image.Length >= MinimumImageSize && image.Length <= MaximumImageSize)
            {
                string imageExtension = Path.GetExtension(image.FileName);

                if (AllowedImageExtensions.Contains(imageExtension) && IsUploadFolderAvailable)
                {
                    string fileName = Guid.NewGuid().ToString() + imageExtension;
                    string filePath = Path.Combine(ImageUploadFolder, fileName);

                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(fileStream);
                    }

                    return fileName;

                }
            }

            return null;
        }

        private List<DownloadLink> DownloadLinksStringToDownloadLinkVM(string downloadLinksString, string linkTitle)
        {
            if (downloadLinksString == null)
            {
                return null;
            }
            else
            {
                linkTitle = linkTitle ?? string.Empty;

                downloadLinksString = new string(downloadLinksString.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());

                List<string> downloadLinksStringList = downloadLinksString.Split(',').ToList();

                List<DownloadLink> downloadLinks = new List<DownloadLink>();

                foreach (string downloadLinkString in downloadLinksStringList)
                {
                    if (!string.IsNullOrEmpty(downloadLinkString))
                    {
                        DownloadLink downloadLink = new DownloadLink()
                        {
                            Title = linkTitle,
                            Link = downloadLinkString,
                            IsActive = true
                        };
                        downloadLinks.Add(downloadLink);
                    }
                }

                return downloadLinks.Count() > 0 ? downloadLinks : null;
            }
        }

        private async Task<List<Screenshot>> SaveScreenshotsAsync(List<IFormFile> images)
        {
            if (images != null && images.Count() > 0)
            {
                List<Screenshot> Screenshots = new List<Screenshot>();

                foreach (IFormFile image in images)
                {
                    string screenshotName = await SaveImageAsync(image);

                    if (screenshotName != null)
                    {
                        Screenshots.Add(new Screenshot()
                        {
                            Image = screenshotName,
                            IsActive = true
                        });
                    }
                }

                if (Screenshots != null && Screenshots.Count() > 0)
                {
                    return Screenshots;
                }
            }

            return null;
        }

        private async Task<List<Category>> CategoriesIdToCategoryListAsync(List<Guid> categoriesId)
        {
            if (categoriesId != null)
            {
                List<Category> Categories = await repository.GetCategoriesAsync(0, false);

                if (Categories.Count() > 0)
                {
                    List<Category> selectedCategories = Categories.Where(x => categoriesId.Contains(x.Id)).ToList();

                    return selectedCategories.Count() > 0 ? selectedCategories : null;
                }
            }

            return null;
        }

        private async Task<List<Game>> GamesIdToGamesListAsync(List<Guid> gamesId)
        {
            if (gamesId != null)
            {
                List<Game> Games = await repository.GetGamesAsync(0, false);

                if (Games.Count() > 0)
                {
                    List<Game> selectedGames = Games.Where(x => gamesId.Contains(x.Id)).ToList();

                    return selectedGames.Count() > 0 ? selectedGames : null;
                }
            }

            return null;
        }

        private void DeleteImage(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string filePath = Path.Combine(ImageUploadFolder, fileName);

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        private void DeleteScreenshots(List<Screenshot> screenshots)
        {
            if (screenshots != null && screenshots.Count() > 0)
            {
                foreach (Screenshot screenshot in screenshots)
                {
                    DeleteImage(screenshot.Image);
                }
            }
        }
    }
}