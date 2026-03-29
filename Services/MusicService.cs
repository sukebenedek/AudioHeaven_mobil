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
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                if (_initialDragFromIndex == -1)
                {
                    _initialDragFromIndex = e.OldStartingIndex;
                    _draggedSong = e.OldItems?[0] as Song;
                }
            }

            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var addedSong = e.NewItems?[0] as Song;

                if (_draggedSong != null && addedSong != null && addedSong.Id == _draggedSong.Id)
                {
                    int currentDropIndex = e.NewStartingIndex;
                    ProcessDebouncedMove(currentDropIndex);
                }
            }

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
            _debounceTimer?.Cancel();

            _debounceTimer = new CancellationTokenSource();
            var token = _debounceTimer.Token;

            Task.Run(async () =>
            {
                await Task.Delay(500, token);

                if (!token.IsCancellationRequested)
                {
                    await API.MoveQueueItemAsync(_initialDragFromIndex + 1, currentDropIndex + 1);

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

        }

        public async Task UpdateQueue()
        {
            var res = await API.GetQueueSongsAsync();

            if (res != null)
            {
                if (Queue != null)
                {
                    Queue.CollectionChanged -= OnQueueChanged;
                }

                Queue = new ObservableCollection<Song>(res);

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
            if (CurrentSong != null)
                History.Add(CurrentSong);

            if (History.Count > 50) 
                History.RemoveAt(0);

            if (Queue != null && Queue.Count > 0)
            {
                CurrentSong = Queue[0];
                PlaySong(CurrentSong);
                await API.DeleteQueuePositionAsync(1);
            }
            else
            {
                var maxAttempts = 5;
                Song? nextSong = null;

                for (int i = 0; i < maxAttempts; i++)
                {
                    var rand = await API.GetReccomendedSongsAsync(1);
                    if (rand == null || rand.Count == 0)
                        break;
                    var candidate = rand[0];
                    if (CurrentSong == null || (candidate.Id != CurrentSong.Id && !History.Any(s => s.Id == candidate.Id))) { 
                        nextSong = candidate;
                        break;
                    }
                }

                if (nextSong == null)
                {
                    var fallback = await API.GetReccomendedSongsAsync(1);
                    nextSong = fallback?.FirstOrDefault();
                }

                if (nextSong != null)
                    PlaySong(nextSong);
            }
            await UpdateQueue();
        }

        public async Task Back()
        {
            //await API.AddToQueueAsync(CurrentSong.Id);
            //await UpdateQueue();

            if (History != null && History.Count > 0)
            {
                var last = History[^1];
                History.RemoveAt(History.Count - 1);
                CurrentSong = last;

                PlaySong(CurrentSong);
                await UpdateQueue();
            }
        }
    }
}