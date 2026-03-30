using AudioHeaven.Models;
using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHeaven.Classes
{
    public class OpenPlayerMessage { }
    public class OpenSongSheetMessage
    {
        public Song Song { get; }

        public OpenSongSheetMessage(Song song)
        {
            Song = song;
        }
    }
    public class RelodadPlaylistsMessage : ValueChangedMessage<int>
    {
        public RelodadPlaylistsMessage(int playlistId) : base(playlistId) { }
    }
}
