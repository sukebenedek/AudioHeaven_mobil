using AudioHeaven.Models;
using AudioHeaven.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHeaven.ViewModels
{

    public partial class SongSheetViewModel : ObservableObject
    {
        [ObservableProperty]
        private MusicService musicService;

        public SongSheetViewModel(MusicService musicService)
        {
            MusicService = musicService;
        }

        public event Action OpenRequested;
        [RelayCommand]
        private void Open()
        {
            WeakReferenceMessenger.Default.Send(new OpenSongSheetMessage(MusicService.SelectedSong));

        }
    }
}
