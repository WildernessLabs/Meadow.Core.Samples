using Meadow;

namespace MauiMeadow
{
    public partial class App : MauiMeadowApplication<Windows>
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}