using Drastic.Services;
using Drastic.Whisper.Services;
using Drastic.WhisperMauiSample.Services;
using Drastic.WhisperSample.ViewModels;
using Microsoft.Extensions.Logging;

namespace Drastic.WhisperMauiSample;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();

		builder.Services.AddSingleton<IAppDispatcher, MauiAppDispatcher>()
            .AddSingleton<IErrorHandlerService, MauiErrorHandler>()
			.AddSingleton<IWhisperService, DefaultWhisperService>()
            .AddSingleton<DebugViewModel>();

		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
