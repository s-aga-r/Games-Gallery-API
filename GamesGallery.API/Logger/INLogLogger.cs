namespace GamesGallery.API.Logger
{
    public interface INLogLogger
    {
        void Information(string message);
        void Warning(string message);
        void Debug(string message);
        void Error(string message);
    }
}
