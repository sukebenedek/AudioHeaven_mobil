using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using AudioHeaven.ViewModels;
using AudioHeaven.Pages;
using Plugin.Maui.BottomSheet.Hosting;

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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddSingleton<MainViewModel>();
            builder.Services.AddSingleton<RegisterPage>();
            builder.Services.AddSingleton<RegisterViewModel>();
            builder.Services.AddSingleton<HomePage>();
            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddSingleton<SearchPage>();
            builder.Services.AddSingleton<SearchViewModel>();
            builder.Services.AddSingleton<LibraryPage>();
            builder.Services.AddSingleton<LibraryViewModel>();
            builder.Services.AddSingleton<MySongsPage>();
            builder.Services.AddSingleton<MySongsViewModel>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
