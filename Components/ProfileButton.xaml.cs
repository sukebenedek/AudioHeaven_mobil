namespace AudioHeaven.Components;

public partial class ProfileButton : ContentView
{
    public event EventHandler Clicked;

    public ProfileButton()
    {
        InitializeComponent();
    }

    private void OnButtonClicked(object sender, EventArgs e)
    {
        // When the internal ImageButton is clicked, tell the "outside world"
        Clicked?.Invoke(this, EventArgs.Empty);
    }
}