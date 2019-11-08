using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Assignment_UWP.Entity;
using Assignment_UWP.Service;
using Newtonsoft.Json;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Assignment_UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CreateSongPage : Page
    {
        private ISongService _songService;
        //private const string SONG_CREATE_URL = "https://2-dot-backup-server-003.appspot.com/_api/v2/songs";
        public CreateSongPage()
        {
            this.InitializeComponent();
        }
        private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Song song = new Song()
            {
                name = Name.Text,
                singer = Singer.Text,
                thumbnail = Thumbnail.Text,
                link = Link.Text
            };
            Dictionary<String, String> errors = song.Validate();
            var uploadSong = this._songService.CreateSong(ProjectConfiguration.CurrentMemberCredential, song);
            if (uploadSong != null)
            {
                Debug.WriteLine("Create success with id: " + uploadSong.Id);
            }
            else
            {
                Debug.WriteLine("Create fails !");
            }
        }
    }
}
