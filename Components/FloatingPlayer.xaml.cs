using AudioHeaven.Classes;
using AudioHeaven.Models;
using AudioHeaven.Services;
using AudioHeaven.ViewModels;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using System.Windows.Input;

namespace AudioHeaven.Components;

public partial class FloatingPlayer : ContentView
{
    private FloatingPlayerViewModel _vm;

    public FloatingPlayer()
	{
		InitializeComponent();

        _vm = Handler?.MauiContext?.Services.GetService<FloatingPlayerViewModel>()
                         ?? App.Current.Handler.MauiContext.Services.GetService<FloatingPlayerViewModel>();

        BindingContext = _vm;

        _vm.OpenRequested += OnOpenRequested;
    }

    private void OnOpenRequested()
    {
        WeakReferenceMessenger.Default.Send(new OpenPlayerMessage());
    }

}