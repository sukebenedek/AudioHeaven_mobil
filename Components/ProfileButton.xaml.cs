
using AudioHeaven.Classes;
using AudioHeaven.Models;

namespace AudioHeaven.Components;

public partial class ProfileButton : ContentView
{
    public event EventHandler Clicked;
    public User? User => UserData.User;

    public ProfileButton()
    {
        InitializeComponent();
        UserData.UserChanged += (_, _) => OnPropertyChanged(nameof(User));
        BindingContext = this;
    }

    private void OnButtonClicked(object sender, EventArgs e)
    {
        Clicked?.Invoke(this, EventArgs.Empty);
    }
}