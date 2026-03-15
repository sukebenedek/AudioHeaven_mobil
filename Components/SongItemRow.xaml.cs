namespace AudioHeaven.Components;

public partial class SongItemRow : ContentView
{
    public static readonly BindableProperty FormatProperty =
        BindableProperty.Create(
            nameof(Format),
            typeof(string),
            typeof(SongItemRow),
            null,
            propertyChanged: OnFormatChanged);

    public string Format
    {
        get => (string)GetValue(FormatProperty);
        set => SetValue(FormatProperty, value);
    }

    public bool ShowPlays { get; set; }
    public bool ShowUsername { get; set; }

    public SongItemRow()
    {
        InitializeComponent();
    }

    static void OnFormatChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (SongItemRow)bindable;

        var format = newValue?.ToString();

        control.ShowPlays = format == "plays";
        control.ShowUsername = format == "username";

        control.OnPropertyChanged(nameof(ShowPlays));
        control.OnPropertyChanged(nameof(ShowUsername));
    }
}