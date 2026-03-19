using AudioHeaven.Services;
using AudioHeaven.ViewModels;
using CommunityToolkit.Maui.Views;

namespace AudioHeaven.Components;

public partial class FloatingPlayer : ContentView
{
	public FloatingPlayer()
	{
		InitializeComponent();

        BindingContext = Handler?.MauiContext?.Services.GetService<FloatingPlayerViewModel>()
                         ?? App.Current.Handler.MauiContext.Services.GetService<FloatingPlayerViewModel>();
    }

    
}