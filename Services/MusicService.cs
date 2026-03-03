using AudioHeaven.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Maui.Views;

namespace AudioHeaven.Services
{
    public partial class MusicService : ObservableObject
    {
        [ObservableProperty]
        private Song currentSong;

        [ObservableProperty]
        private bool isPlaying;

        [ObservableProperty]
        private bool isPlayerVisible;

        public MediaElement InternalPlayer { get; set; }

        public void Toggle()
        {
            if (InternalPlayer == null) return;

            if (InternalPlayer.CurrentState == CommunityToolkit.Maui.Core.Primitives.MediaElementState.Playing)
            {
                InternalPlayer.Pause();
                IsPlaying = false;
            }
            else
            {
                InternalPlayer.Play();
                IsPlaying = true;
            }
        }
    }
}