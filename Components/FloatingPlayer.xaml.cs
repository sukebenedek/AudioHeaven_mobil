using AudioHeaven.Services;
using AudioHeaven.ViewModels;
using CommunityToolkit.Maui.Views;

namespace AudioHeaven.Components;

public partial class FloatingPlayer : ContentView
{
	public FloatingPlayer()
	{
		InitializeComponent();

        BindingContext = Handler?.MauiContext?.Services.GetService<FloatingPlayerViewModel>()
                         ?? App.Current.Handler.MauiContext.Services.GetService<FloatingPlayerViewModel>();
    }

    private void musicPlayer_Loaded(object sender, EventArgs e)
    {
        //var musicService = IPlatformApplication.Current.Services.GetService<MusicService>();
        //if (musicService != null)
        //{
        //    // Link the player
        //    musicService.InternalPlayer = (MediaElement)sender;

        //    // Check if a song was already picked while we were loading
        //    if (musicService.CurrentSong != null && musicService.IsPlaying)
        //    {
        //        musicService.InternalPlayer.Source = MediaSource.FromUri(musicService.CurrentSong.FullAudioUrl);
        //        musicService.InternalPlayer.Play();
        //    }
        //}
    }
}