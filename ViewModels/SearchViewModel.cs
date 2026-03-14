using AudioHeaven.Classes;
using AudioHeaven.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioHeaven.ViewModels
{
    public partial class SearchViewModel : ObservableObject
    {
            [ObservableProperty]
            private string _searchText;

            [ObservableProperty]
            private ObservableCollection<Album> albums = new();

            [ObservableProperty]
            [NotifyPropertyChangedFor(nameof(IsStartVisible))]
            private bool hasAlbums = false;

            [ObservableProperty]
            [NotifyPropertyChangedFor(nameof(IsStartVisible))]
            private bool hasSongs = false;

            [ObservableProperty]
            [NotifyPropertyChangedFor(nameof(IsStartVisible))]
            private bool hasUsers = false;

            public bool IsStartVisible => !HasAlbums && !HasSongs && !HasUsers;


        // This method is automatically called by the Toolkit when SearchText changes
        partial void OnSearchTextChanged(string value)
            {
                HasAlbums = Albums.Count() != 0;

                if (string.IsNullOrWhiteSpace(value))
                {
                    Albums.Clear();
                    HasAlbums = false;
                    return;
                }

                // Trigger the async search
                _ = SearchAlbumsAsync(value);
                
            }

            private async Task SearchAlbumsAsync(string query)
            {
                // Optional: Add a small delay (300ms) to wait for the user to stop typing
                await Task.Delay(300);
                if (query != SearchText) return; // If text changed again, cancel this run
                 //await Shell.Current.DisplayAlert("Error", query, "Ok");

                UserData.SearchTerm = query;
                var results = await API.GetAlbumsSearchAsync(query, 5);
                HasAlbums = results.Count() != 0;

                if (results != null)
                {
                    Albums.Clear();
                    foreach (var album in results)
                    {
                        Albums.Add(album);
                    }
                }
        }
    }
}
