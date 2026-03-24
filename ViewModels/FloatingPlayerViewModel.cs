using AudioHeaven.Classes;
using AudioHeaven.Models;
using AudioHeaven.Services; 
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

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

        [RelayCommand]
        private async Task GoToUserPage() // Renamed for convention and made Task
        {
            if (MusicService.CurrentSong?.User != null)
            {
                await Shell.Current.GoToAsync($"UserPage?id={MusicService.CurrentSong.User.Id}");
            }
        }

        [RelayCommand]
        private async Task GoToSongPage() // Renamed for convention and made Task
        {
            if (MusicService.CurrentSong != null)
            {
                await Shell.Current.GoToAsync($"SongPage?id={MusicService.CurrentSong.Id}");
            }
        }

        public event Action OpenRequested;
        [RelayCommand]
        private void Open()
        {
            WeakReferenceMessenger.Default.Send(new OpenPlayerMessage());
            MusicService.GetCurrentQueueSong();

        }
    }
}