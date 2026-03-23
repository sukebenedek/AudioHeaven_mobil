
using AudioHeaven.Classes;

namespace AudioHeaven.Components;

public partial class ProfileButton : ContentView
{
    public event EventHandler Clicked;

    public ProfileButton()
    {
        InitializeComponent();
        ProfileImage.BindingContext = UserData.User;
    }

    private void OnButtonClicked(object sender, EventArgs e)
    {
        Clicked?.Invoke(this, EventArgs.Empty);
    }
}