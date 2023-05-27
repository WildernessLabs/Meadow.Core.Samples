using Microsoft.Extensions.Logging;

namespace MauiMeadow
{
    public static class MauiProgram
    {
#if DISABLE_XAML_GENERATED_MAIN
        public static class Program
        {
            [global::System.Runtime.InteropServices.DllImport("Microsoft.ui.xaml.dll")]
            private static extern void XamlCheckProcessRequirements();

            [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.UI.Xaml.Markup.Compiler", " 1.0.0.0")]
            [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
            [global::System.STAThreadAttribute]
            static void Main(string[] args)
            {
                XamlCheckProcessRequirements();

                global::WinRT.ComWrappersSupport.InitializeComWrappers();
                global::Microsoft.UI.Xaml.Application.Start((p) => {
                    var context = new global::Microsoft.UI.Dispatching.DispatcherQueueSynchronizationContext(global::Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread());
                    global::System.Threading.SynchronizationContext.SetSynchronizationContext(context);
                    new App();
                });
            }
        }
#endif
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

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}