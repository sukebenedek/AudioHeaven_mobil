using AudioHeaven.Classes;
using AudioHeaven.Models;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading;

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

        private int _dragFromIndex = -1;
        private Song _draggedSong = null;
        private int _initialDragFromIndex = -1;
        private CancellationTokenSource _debounceTimer = null;

        private MediaElement _player;
        private Song _pendingSong;
        public MusicService()
        {
            Queue.CollectionChanged += OnQueueChanged;
        }

        private void OnQueueChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // 1. Android/iOS Quirk: Catch the 'Remove' part of the drag
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                // ONLY save the initial index if we aren't already in the middle of a drag
                if (_initialDragFromIndex == -1)
                {
                    _initialDragFromIndex = e.OldStartingIndex;
                    _draggedSong = e.OldItems?[0] as Song;
                }
            }

            // 2. Android/iOS Quirk: Catch the 'Add' part of the drag
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var addedSong = e.NewItems?[0] as Song;

                // If it's the same song we are dragging...
                if (_draggedSong != null && addedSong != null && addedSong.Id == _draggedSong.Id)
                {
                    int currentDropIndex = e.NewStartingIndex;

                    // Trigger the Debounce logic
                    ProcessDebouncedMove(currentDropIndex);
                }
            }

            // 3. Native Move (Windows/Mac)
            else if (e.Action == NotifyCollectionChangedAction.Move)
            {
                if (_initialDragFromIndex == -1)
                {
                    _initialDragFromIndex = e.OldStartingIndex;
                }

                ProcessDebouncedMove(e.NewStartingIndex);
            }
        }

        private void ProcessDebouncedMove(int currentDropIndex)
        {
            // 1. Cancel the previous timer if they are still dragging!
            _debounceTimer?.Cancel();

            // 2. Create a fresh timer
            _debounceTimer = new CancellationTokenSource();
            var token = _debounceTimer.Token;

            // 3. Start a background countdown
            Task.Run(async () =>
            {
                // Wait for 500 milliseconds...
                await Task.Delay(500, token);

                // 4. If 500ms passes and this task WASN'T cancelled, they dropped it!
                if (!token.IsCancellationRequested)
                {
                    // Call the API with the ORIGINAL start index, and the FINAL drop index
                    await API.MoveQueueItemAsync(_initialDragFromIndex + 1, currentDropIndex + 1);

                    // Reset our trackers for the next time they drag something
                    _initialDragFromIndex = -1;
                    _draggedSong = null;
                }
            }, token);
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