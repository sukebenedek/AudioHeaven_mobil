using AudioHeaven.Classes;
using AudioHeaven.Models;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace AudioHeaven.Services
{
    public partial class MusicService : ObservableObject
    {
        [ObservableProperty]
        private Song currentSong;

        [ObservableProperty]
        private Song selectedSong;

        [ObservableProperty]
        private ObservableCollection<Song> queue = new();

        [ObservableProperty]
        private ObservableCollection<Song> history = new();

        [ObservableProperty]
        private bool hasQueue = false;

        [ObservableProperty]
        private Song currentQueueSong;

        [ObservableProperty]
        private bool isPlaying;

        [ObservableProperty]
        private bool isPlayerVisible = false;

        private Song _draggedSong = null;
        private int _dragFromIndex = -1;

        private MediaElement _player;
        private Song _pendingSong;
        public MusicService()
        {
            Queue.CollectionChanged += OnQueueChanged;
        }

        private async void OnQueueChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // 1. Native Move (This still runs perfectly if you run the app on Windows)
            if (e.Action == NotifyCollectionChangedAction.Move)
            {
                await API.MoveQueueItemAsync(e.OldStartingIndex + 1, e.NewStartingIndex + 1);
                return;
            }

            // 2. Android/iOS Quirk: Catch the 'Remove' part of the drag
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                // Save the song that is being picked up, and its original position
                _draggedSong = e.OldItems?[0] as Song;
                _dragFromIndex = e.OldStartingIndex;
            }

            // 3. Android/iOS Quirk: Catch the 'Add' part of the drag
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var addedSong = e.NewItems?[0] as Song;

                // If the item being dropped in is the exact same one that was just picked up... we have a Move!
                // (Using .Id is bulletproof here)
                if (_draggedSong != null && addedSong != null && addedSong.Id == _draggedSong.Id)
                {
                    int toIndex = e.NewStartingIndex;

                    // Call your backend! (Keeping your +1 logic for Laravel)
                    await API.MoveQueueItemAsync(_dragFromIndex + 1, toIndex + 1);
                }

                // Always reset the trackers so normal deleting/adding later doesn't break
                _draggedSong = null;
                _dragFromIndex = -1;
            }
        }

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

        private async void PlayInternal(Song song)
        {
            if (_player == null) return;

            _player.Source = MediaSource.FromUri(song.FullAudioUrl);
            _player.Play();
            IsPlaying = true;

            //History.Insert(0, song);
        }

        public async Task UpdateQueue()
        {
            var res = await API.GetQueueSongsAsync();

            if (res != null)
            {
                // 1. Unsubscribe from the old collection to prevent memory leaks
                if (Queue != null)
                {
                    Queue.CollectionChanged -= OnQueueChanged;
                }

                // 2. Create the new ObservableCollection with the API results
                Queue = new ObservableCollection<Song>(res);

                // 3. Re-attach the listener to the newly created queue!
                Queue.CollectionChanged += OnQueueChanged;

                HasQueue = Queue.Count > 0;

                if (Queue.Count > 0)
                {
                    CurrentQueueSong = Queue[0];
                }
            }
        }

        public async Task Skip()
        {
            if (Queue != null && Queue.Count > 0)
            {
                CurrentSong = Queue[0];
                PlaySong(CurrentSong);
                await API.DeleteQueuePositionAsync(1);
                await UpdateQueue();
            }
        }
    }
}