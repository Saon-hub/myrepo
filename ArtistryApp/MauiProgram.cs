using ArtClass.Mobile.Services;
using ArtClass.Mobile.Views;
using ArtistryApp.Views;
using Microsoft.Extensions.Logging;

namespace ArtistryApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.Services.AddSingleton<App>();
            builder.Services.AddHttpClient<CalendarService>(client =>
            {
                client.BaseAddress =
                    new Uri("https://artistry-api-h8d6bbdshhf9ffes.centralindia-01.azurewebsites.net/");
            });
            builder.Services.AddHttpClient<ClassStudentService>(client =>
            {
                client.BaseAddress =
                    new Uri("https://artistry-api-h8d6bbdshhf9ffes.centralindia-01.azurewebsites.net/");
            });
           
            builder.Services.AddTransient<CalendarPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ClassStudentPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
