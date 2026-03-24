using AudioHeaven.Pages;
using AudioHeaven.Views;
using AudioHeaven.Services;

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

            Routing.RegisterRoute("SeachedSongsPage", typeof(SearchedSongsPage));
            Routing.RegisterRoute("SeachedAlbumsPage", typeof(SearchedAlbumsPage));
            Routing.RegisterRoute("AllSongsPage", typeof(AllSongsPage));
            Routing.RegisterRoute("SongListPage", typeof(SongListPage));

            Routing.RegisterRoute("UserPage", typeof(UserPage));
            Routing.RegisterRoute("AlbumPage", typeof(AlbumPage));
            Routing.RegisterRoute("SongPage", typeof(SongPage));

        }
    }
}
