using AudioHeaven.Classes;

namespace AudioHeaven.Components;

public partial class LogoutSheetContent : ContentView
{
    public event EventHandler LogoutRequested;

    public LogoutSheetContent()
    {
        InitializeComponent();
    }

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        LogoutRequested?.Invoke(this, EventArgs.Empty);
    }

}