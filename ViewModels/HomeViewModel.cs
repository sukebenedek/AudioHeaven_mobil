using AudioHeaven.Classes;
using AudioHeaven.Models;
using AudioHeaven.Services;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHeaven.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty]
        private MusicService musicService;

        public HomeViewModel(MusicService musicService)
        {
            MusicService = musicService;
        }

        [RelayCommand]
        public void Play(object? parameter)
        {
            if (parameter is MediaElement player)
            {
                if (player.CurrentState == MediaElementState.Playing)
                    player.Pause();
                else
                    player.Play();
            }
        }

        [ObservableProperty]
        private ObservableCollection<Song> history = new();

        [ObservableProperty]
        private ObservableCollection<Song> reccomended = new();

        [ObservableProperty]
        private ObservableCollection<Song> discover = new();

        [ObservableProperty]
        private bool hasHistory;

        [ObservableProperty]
        private bool isBusy = true;

        public async Task LoadOnceAsync()
        {
            //History = new ObservableCollection<Song>(await API.GetUserHistoryAsync()).OrderByDescending(s => s.Plays).Take(10).ToObservableCollection();
            //HasHistory = History.Any();
            Reccomended = new ObservableCollection<Song>(await API.GetDiscoverAsync(7));
            IsBusy = false;
        }

        public async Task LoadAlwaysAsync()
        {
            isBusy = true;
            History = new ObservableCollection<Song>(await API.GetHistoryAsync(1));
            HasHistory = History.Any();
            IsBusy = false;
        }
    }
}
