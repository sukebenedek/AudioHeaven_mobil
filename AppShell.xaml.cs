using AudioHeaven.Pages;

namespace AudioHeaven
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
            Routing.RegisterRoute("HomePage", typeof(HomePage));
            Routing.RegisterRoute("SearchPage", typeof(SearchPage));
            Routing.RegisterRoute("MySongsPage", typeof(MySongsPage));
            Routing.RegisterRoute("LibraryPage", typeof(LibraryPage));
        }
    }
}
