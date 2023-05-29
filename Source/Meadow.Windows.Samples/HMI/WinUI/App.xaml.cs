// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Meadow;
using Microsoft.UI.Xaml;

namespace WinUIMeadow
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : WinUIMeadowApplication
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.LoadMeadowOS();
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
            // add this to the resolver so the app can use it for invoking
            Resolver.Services.Add<Window>(m_window);
        }

        private Window m_window;
    }
}
