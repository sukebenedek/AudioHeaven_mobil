namespace AudioHeaven.Components;

public partial class UserItemCard : ContentView
{
	public UserItemCard()
	{
		InitializeComponent();
	}

    private async void OnUserClicked(object sender, EventArgs e)
    {
        if (BindingContext is Models.User clickedUser)
        {
            // We create a dictionary to pass the object
            var navigationParameter = new Dictionary<string, object>
            {
                { "SelectedUser", clickedUser }
            };

            // Navigate to your UserPage (make sure this route is registered in AppShell.xaml.cs)
            await Shell.Current.GoToAsync("UserPage", navigationParameter);
        }
    }
}