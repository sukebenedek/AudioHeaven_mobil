using AudioHeaven.Models;

namespace AudioHeaven.Components;

public partial class AlbumItemRow : ContentView
{
	public AlbumItemRow()
	{
		InitializeComponent();
	}

    public static readonly BindableProperty FormatProperty =
        BindableProperty.Create(
            nameof(Format),
            typeof(string),
            typeof(AlbumItemRow),
            null,
            propertyChanged: OnFormatChanged);

    public string Format
    {
        get => (string)GetValue(FormatProperty);
        set => SetValue(FormatProperty, value);
    }

    public bool ShowDate { get; set; }
    public bool ShowUsername { get; set; } = true;
    public bool IsCard { get; set; }

    static void OnFormatChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (AlbumItemRow)bindable;

        var format = newValue?.ToString();

        control.ShowDate = format == "date";
        control.ShowUsername = format == "username" || format == null;
        control.IsCard = format == "card";

        control.OnPropertyChanged(nameof(ShowDate));
        control.OnPropertyChanged(nameof(ShowUsername));
        control.OnPropertyChanged(nameof(IsCard));
    }

    private async void OnHeaderClicked(object sender, EventArgs e)
    {
        if (sender is BindableObject bindable && bindable.BindingContext is Album album)
        {
            await Shell.Current.GoToAsync($"AlbumPage?id={album.Id}");
        }
    }

    private async void OnGoToUserPageClicked(object sender, EventArgs e)
    {
        if (BindingContext is Album a && a != null)
        {
            await Shell.Current.GoToAsync($"UserPage?id={a.UserId}");
        }
    }
}