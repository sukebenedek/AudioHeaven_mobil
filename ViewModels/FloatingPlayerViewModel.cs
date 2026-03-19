using AudioHeaven.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Views;
using AudioHeaven.Classes;
using AudioHeaven.Services; 

namespace AudioHeaven.ViewModels
{
    public partial class FloatingPlayerViewModel : ObservableObject
    {
        [ObservableProperty]
        private MusicService musicService;

        public FloatingPlayerViewModel(MusicService musicService)
        {
            MusicService = musicService;
        }

        [RelayCommand]
        private void TogglePlay()
        {
            MusicService.Toggle();
        }
    }
}