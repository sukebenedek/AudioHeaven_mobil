using AudioHeaven.Pages;

namespace AudioHeaven
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("RegisterPage", typeof(RegisterPage));
            //Routing.RegisterRoute("HomePage", typeof(HomePage));
        }
    }
}
