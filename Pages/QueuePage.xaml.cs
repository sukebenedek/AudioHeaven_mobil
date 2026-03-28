using AudioHeaven.ViewModels;

namespace AudioHeaven.Pages;

public partial class QueuePage : ContentPage
{
	private QueueViewModel _vm;
    public QueuePage(QueueViewModel vm)
	{
		InitializeComponent();
		_vm = vm;
		this.BindingContext = _vm;
	}

    protected override async void OnAppearing()
    {
            await _vm.UpdateSongs();
    }
}