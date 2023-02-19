﻿using Microsoft.Extensions.Logging;

namespace Alohakit.Components.Gallery
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>();

#if DEBUG
		    builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}