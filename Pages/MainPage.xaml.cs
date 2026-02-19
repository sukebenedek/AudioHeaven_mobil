using AudioHeaven.ViewModels;

namespace AudioHeaven
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel vm)
        {
            InitializeComponent();
            this.BindingContext = vm;
        }
    }
}
