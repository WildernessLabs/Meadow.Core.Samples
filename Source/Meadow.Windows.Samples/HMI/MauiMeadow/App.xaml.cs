namespace MauiMeadow
{
    public partial class App : MauiMeadowApplication<Meadow.Windows>
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}