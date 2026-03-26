using AudioHeaven.Classes;
using AudioHeaven.Models;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AudioHeaven.Services
{
    public partial class MusicService : ObservableObject
    {
        [ObservableProperty]
        private Song currentSong;

        [ObservableProperty]
        private Song selectedSong;

        [ObservableProperty]
        private List<Song> queue;

        [ObservableProperty]
        private bool hasQueue = false;

        [ObservableProperty]
        private Song currentQueueSong;

        [ObservableProperty]
        private bool isPlaying;

        [ObservableProperty]
        private bool isPlayerVisible = false;

        private MediaElement _player;
        private Song _pendingSong;

        public void SetPlayer(MediaElement player)
        {
            _player = player;

            _player.StateChanged += OnPlayerStateChanged;

            if (_pendingSong != null)
            {
                PlayInternal(_pendingSong);
                _pendingSong = null;
            }
        }
        private void OnPlayerStateChanged(object sender, EventArgs e)
        {
            if (_player == null) return;

            IsPlaying = _player.CurrentState == MediaElementState.Playing;
        }
        public void Toggle()
        {
            if (_player == null) return;

            if (IsPlaying)
            {
                _player.Pause();
                IsPlaying = false;
            }
            else
            {
                _player.Play();
                IsPlaying = true;
            }
        }

        public async void PlaySong(Song song)
        {
            CurrentSong = song;
            IsPlayerVisible = true;
            API.LogPlayAsync(CurrentSong.Id);
            if (_player == null)
            {
                _pendingSong = song;
                return;
            }

            PlayInternal(song);
        }

        private void PlayInternal(Song song)
        {
            if (_player == null) return;

            _player.Source = MediaSource.FromUri(song.FullAudioUrl);
            _player.Play();
            IsPlaying = true;
        }

        public async Task UpdateQueue()
        {
            var res = await API.GetQueueSongsAsync();
            if (res != null)
            {
                Queue = res.ToList();
                HasQueue = Queue.Count > 0;
                if (res.Count > 0)
                {
                    CurrentQueueSong = res[0];
                }
            }
        }
    }
}