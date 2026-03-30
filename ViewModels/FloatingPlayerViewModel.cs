using AudioHeaven.Classes;
using AudioHeaven.Models;
using AudioHeaven.Services; 
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Threading.Tasks;

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
        private async Task Skip()
        {   
            await MusicService.Skip();
        }

        [RelayCommand]
        private async Task Back()
        {
            await MusicService.Back();
        }

        [RelayCommand]
        private async Task GoToUserPage() 
        {
            if (MusicService.CurrentSong?.User != null)
            {
                await Shell.Current.GoToAsync($"UserPage?id={MusicService.CurrentSong.User.Id}");
            }
        }

        [RelayCommand]
        private async Task GoToSongPage() 
        {
            if (MusicService.CurrentSong != null)
            {
                if (MusicService.CurrentSong.AlbumId != null)
                {
                    await Shell.Current.GoToAsync($"AlbumPage?id={MusicService.CurrentSong.AlbumId}");
                    return;
                }
                await Shell.Current.GoToAsync($"SongPage?id={MusicService.CurrentSong.Id}");
            }
        }

        public event Action OpenRequested;
        [RelayCommand]
        private async void Open()
        {
            WeakReferenceMessenger.Default.Send(new OpenPlayerMessage());
            await MusicService.UpdateQueue();

        }
    }
}