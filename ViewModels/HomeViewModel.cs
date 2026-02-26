using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core.Primitives;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHeaven.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
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
    }
}
