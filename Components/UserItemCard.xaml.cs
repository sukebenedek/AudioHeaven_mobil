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
            await Shell.Current.GoToAsync($"UserPage?id={clickedUser.Id}");
        }
    }
}