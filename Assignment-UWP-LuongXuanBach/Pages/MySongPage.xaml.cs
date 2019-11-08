using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Assignment_UWP.Entity;
using Assignment_UWP.Service;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Assignment_UWP.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MySongPage : Page
    {
        private List<Song> _listSongs;
        private int _currentIndex = 0;
        private bool _isPlaying;
        private DispatcherTimer _playTimer;
        private IFileService fileService;
        public MySongPage()
        {
            Debug.WriteLine("Init My song.");
            fileService.ReadMemberCredentialFromFile();
            this.InitializeComponent();
        }
        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            var chooseSong = sender as StackPanel;
            MediaPlayer.Pause();
            if (chooseSong != null)
            {
                if (chooseSong.Tag is Song currentSong)
                {
                    _currentIndex = _listSongs.IndexOf(currentSong);
                    MediaPlayer.Source = new Uri(currentSong.link);
                    NowPlayingText.Text = "Now playing: " + currentSong.name + " - " + currentSong.singer;
                    Play();
                }
            }

            MediaPlayer.Play();
            Media_StateChanged();
        }

        private void Media_StateChanged()
        {
            if (MediaPlayer.CurrentState == MediaElementState.Playing)
            {
                progressBar1.Maximum = MediaPlayer.Position.Duration().TotalSeconds;
                _playTimer.Start();
            }
            else if (MediaPlayer.CurrentState == MediaElementState.Paused)
            {
                _playTimer.Stop();
            }
            else if (MediaPlayer.CurrentState == MediaElementState.Stopped)
            {
                _playTimer.Stop();
                progressBar1.Value = 0;
            }
        }

        private void Play()
        {
            MediaPlayer.Source = new Uri(_listSongs[_currentIndex].link);
            NowPlayingText.Text = "Now playing: " + _listSongs[_currentIndex].name + " - " +
                                  _listSongs[_currentIndex].singer;
            TopSongs.SelectedIndex = _currentIndex;
            MediaPlayer.Play();
            PlayButton.Icon = new SymbolIcon(Symbol.Pause);
            _isPlaying = true;
            if (_isPlaying)
            {
                _playTimer = new DispatcherTimer();
                _playTimer.Interval = new TimeSpan(0, 0, 0);
                _playTimer.Tick += new EventHandler<object>(playTimer_Tick);
                _playTimer.Start();
            }
        }

        private void playTimer_Tick(object sender, object e)
        {
            TimeStart.Text = MediaPlayer.Position.ToString();
            TimeEnd.Text = MediaPlayer.NaturalDuration.TimeSpan.ToString();
            if (MediaPlayer.CurrentState == MediaElementState.Playing)
            {
                progressBar1.Value = MediaPlayer.Position.Seconds;
            }

        }

        private void PreviousButton_OnClick(object sender, RoutedEventArgs e)
        {
            _currentIndex--;
            if (_currentIndex < 0)
            {
                _currentIndex = _listSongs.Count - 1;
            }
            else if (_currentIndex >= _listSongs.Count)
            {
                _currentIndex = 0;
            }
            Play();
        }

        private void PlayButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_isPlaying)
            {
                Pause();
            }
            else
            {
                Play();
            }
        }

        private void Pause()
        {
            MediaPlayer.Pause();
            PlayButton.Icon = new SymbolIcon(Symbol.Play);
            _isPlaying = false;
        }

        private void NextButton_OnClick(object sender, RoutedEventArgs e)
        {
            _currentIndex++;
            if (_currentIndex >= _listSongs.Count || _currentIndex < 0)
            {
                _currentIndex = 0;
            }
            Play();
        }
    }
}
