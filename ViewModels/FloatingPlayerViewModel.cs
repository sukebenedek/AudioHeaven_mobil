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
        private void TogglePlay(MediaElement player)
        {
            // Link the UI player to the service instance
            if (MusicService.InternalPlayer == null)
                MusicService.InternalPlayer = player;

            MusicService.Toggle();
        }
    }
}