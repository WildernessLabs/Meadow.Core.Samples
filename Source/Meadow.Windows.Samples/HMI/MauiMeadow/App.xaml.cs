using Meadow;

namespace MauiMeadow
{
    public partial class App : MauiMeadowApplication<Meadow.Windows>
    {
        public App()
        {
            InitializeComponent();
            LoadMeadowOS();

            MainPage = new AppShell();
        }
    }
}