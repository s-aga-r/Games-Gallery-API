using AutoMapper;
using GamesGallery.DL;
using GamesGallery.VM;
using GamesGallery.VM.CreateVM;

namespace GamesGallery.API.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Game, GameVM>().ReverseMap();
            CreateMap<Slider, SliderVM>().ReverseMap();
            CreateMap<Game, CreateGameVM>().ReverseMap();
            CreateMap<Category, CategoryVM>().ReverseMap();
            CreateMap<Slider, CreateSliderVM>().ReverseMap();
            CreateMap<Screenshot, ScreenshotVM>().ReverseMap();
            CreateMap<Category, CreateCategoryVM>().ReverseMap();
            CreateMap<DownloadLink, DownloadLinkVM>().ReverseMap();
            CreateMap<Screenshot, CreateScreenshotVM>().ReverseMap();
            CreateMap<DownloadLink, CreateDownloadLinkVM>().ReverseMap();
        }
    }
}
