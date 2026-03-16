using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using AudioHeaven.ViewModels;
using AudioHeaven.Pages;
using AudioHeaven.Views;
using Plugin.Maui.BottomSheet.Hosting;
using AudioHeaven.Services;

namespace AudioHeaven
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseBottomSheet()
                .UseMauiCommunityToolkitMediaElement()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<RegisterPage>();
            builder.Services.AddSingleton<SearchPage>();
            builder.Services.AddSingleton<LibraryPage>();
            builder.Services.AddSingleton<MySongsPage>();
            builder.Services.AddSingleton<AllSongsPage>();

            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<RegisterViewModel>();
            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddSingleton<SearchViewModel>();
            builder.Services.AddSingleton<LibraryViewModel>();
            builder.Services.AddSingleton<MySongsViewModel>();
            builder.Services.AddSingleton<FloatingPlayerViewModel>();

            builder.Services.AddSingleton<MusicService>();

            builder.Services.AddSingleton<SearchedAlbumsPage>();
            builder.Services.AddSingleton<SearchedSongsPage>();

            //builder.Services.AddSingleton<UserPage>();
            //builder.Services.AddSingleton<AlbumPage>();
            //builder.Services.AddSingleton<SongPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
