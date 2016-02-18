using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Awesomium.Core;
using Awesomium.Windows.Controls;

namespace Pizza
{
    public delegate void UiDelegate();
    public delegate void BrowsweDispatcherDelegate();
    
    public partial class MainWindow
    {
        #region Fields                      
        private bool _nowplaying;
        private string _selectedtrack;
        private string _downloadedtrack;
        private readonly DispatcherTimer _timer;
        #endregion
        public MainWindow()
        {   
            InitializeComponent();
            Keys.RestoreKeys();//Десериализация ключей (token, id)
            if (Keys.IsKeyEmpty())
            {
                VkRequest.CreateBro(this);
            }
            VkRequest.CreateRequestAndRefreshUi();
            VkRequest.ReqquestError += ForceRelogin;//Событие генерирует вызову функции принудительного релогина при невалидном токене.
            VkRequest.RefreshUi += AsyncUiUpdate;//Событие генерирует функцию асинхронного обновления Ui.
            _timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(100)};
            _timer.Tick += TimerTick;                            
        }
        #region Functions    
        private void ForceRelogin()
        {
            var bd = new UiDelegate(CreateBrowserAsync);
            Dispatcher.Invoke(bd);
        } 
        private void CreateBrowserAsync()
        {
            VkRequest.CreateBro(this);
        }
        private void AsyncUiUpdate()
        {
            var ud = new UiDelegate(UiLoad);
            Dispatcher.Invoke(ud);
        }
        private void UiLoad()
        {
            ListMain.ItemsSource = VkRequest.AudioResult();
        }
        private void PlayNextTrack()
        {
            if (ListMain.Items.Count == 0)
                return;
            if (ListMain.SelectedIndex > ListMain.Items.Count)
                return;
            ListMain.SelectedIndex++;
            var nexttrackselected = ListMain.SelectedItem as AudioResult;
            if (nexttrackselected != null) _selectedtrack = nexttrackselected.Url;
            var snexttrackselected = ListMain.SelectedItem as SearchResult;
            if (snexttrackselected != null) _selectedtrack = snexttrackselected.Url;
            MediaElement.Source = new Uri(_selectedtrack);
            MediaElement.Play();
            _nowplaying = true;            
        }
        private void Shuffle()
        {
            var r = new Random();
            ListMain.SelectedIndex = r.Next(0, ListMain.Items.Count);
            MediaElement.Source = new Uri(_selectedtrack);
            MediaElement.Play();
            _nowplaying = true;
        }
        private void PlayPreviousTrack()
        {
            if (ListMain.SelectedIndex < 1) { return; }                
            ListMain.SelectedIndex--;
            var previoustrackselected = ListMain.SelectedItem as AudioResult;
            var sprevioustrackselected = ListMain.SelectedItem as SearchResult;
            if (previoustrackselected != null) _selectedtrack = previoustrackselected.Url;
            if (sprevioustrackselected != null) _selectedtrack = sprevioustrackselected.Url;
            MediaElement.Source = new Uri(_selectedtrack);
            MediaElement.Play();
            _nowplaying = true;
        }
        #endregion
        #region ButtonsEventHandler
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (_nowplaying != false) return;
            MediaElement.Play();
            _nowplaying = true;
        }
        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (_nowplaying != true) return;
            MediaElement.Stop();
            _nowplaying = false;
        }
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (_nowplaying != true) return;
            MediaElement.Pause();
            _nowplaying = false;
        }
        private void PreviousTrackButton_Click(object sender, RoutedEventArgs e)
        {
            PlayPreviousTrack();
        }
        private void NexttackButton_Click(object sender, RoutedEventArgs e)
        {
            PlayNextTrack();
        }
        private void TestButton_OnClick(object sender, RoutedEventArgs e)
        {
            ListMain.ItemsSource = null;
            VkRequest.Logout(this);           
            VkRequest.CreateBro(this);
        }
        private void DownloadButton_OnClick(object sender, RoutedEventArgs e)
        {
            var trackselected = ListMain.SelectedItem as AudioResult;
            var strackselected = ListMain.SelectedItem as SearchResult;
            var d = new Downloader();
            if (trackselected!=null)
            {
                d.Download(trackselected.Url, trackselected.ArtistAndTitle);
            }
            if (strackselected != null)
            {
                d.Download(strackselected.Url, strackselected.ArtistAndTitle);
            }
        }
        #endregion
        #region SlidersEnventHadler
        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaElement.Volume = VolumeSlider.Value;
        }
        private void ProgresSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            MediaElement.Position = TimeSpan.FromMilliseconds(ProgresSlider.Value);
            CurrentTimeLabel.Content = string.Format(@"{0:mm\:ss}", TimeSpan.FromMilliseconds(ProgresSlider.Value));
        }
        #endregion
        #region UIEventHandler
        private void MediaElement_OnMediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show(e.Source.ToString());
        }
        private void MediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            var trackselected = ListMain.SelectedItem as AudioResult;
            if (trackselected != null)
                IcebergTextLabel.Content = trackselected.ArtistAndTitle;
            var searchedtrack = ListMain.SelectedItem as SearchResult;
            if (searchedtrack != null)
                IcebergTextLabel.Content = searchedtrack.ArtistAndTitle;
            var dbtrackselected = ListMain.SelectedItem as SqLiteDataObjects;
            if (dbtrackselected != null)
                IcebergTextLabel.Content = dbtrackselected.ArtistAndTitle;
            ProgresSlider.Maximum = MediaElement.NaturalDuration.TimeSpan.TotalMilliseconds;
            TotalTimelabel.Content = $@"{MediaElement.NaturalDuration.TimeSpan:mm\:ss}";
            _timer.Start();
        }
        private void TimerTick(object sender, EventArgs e)
        {
            ProgresSlider.Value = MediaElement.Position.TotalMilliseconds;
        }
        private void ListMain_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (ListMain.Items.Count == 0)return;
            var trackselected = ListMain.SelectedItem as AudioResult;
            var searchedtrack = ListMain.SelectedItem as SearchResult;
            var dbtrach = ListMain.SelectedItem as SqLiteDataObjects; 
            if (trackselected != null)
            {
                _selectedtrack = trackselected.Url;
            }
            if (searchedtrack != null)
            {
                _selectedtrack = searchedtrack.Url;
            }
            if (dbtrach != null)
            {
                _selectedtrack = dbtrach.Url;
            }            
            MediaElement.Source = new Uri(_selectedtrack);
            MediaElement.Play();
            _nowplaying = true;
        }
        private void MediaElement_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            if (ShuffleButton.IsChecked == true)
            {
                Shuffle();
            }
            if (RepeatButton.IsChecked == true)
            {
                MediaElement.Source = new Uri(_selectedtrack);
                MediaElement.Play();
                _nowplaying = true;
            }
            else
            {
                PlayNextTrack();
            }
        }
        private void SearchBox_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && SearchBox.Text != string.Empty)
            {
                VkRequest.Search(SearchBox.Text);
                ListMain.ItemsSource = VkRequest.SearchResults();
            }
            if (e.Key == Key.Return && SearchBox.Text == string.Empty)
            {
                VkRequest.Search(SearchBox.Text);
                VkRequest.CreateRequestAndRefreshUi();
            }
        }
        private void LogoutButton_MouseEnter(object sender, MouseEventArgs e)
        {
            //var imagesource = new Uri(@"pack://application:,,,/WPFResources/Change User-64.png", UriKind.Absolute);
            //var myBrush = new ImageBrush { ImageSource = new BitmapImage(new Uri(imagesource.AbsoluteUri, UriKind.Absolute)) };
            //LogoutButton.Background = myBrush;
        }
        private void LogoutButton_MouseLeave(object sender, MouseEventArgs e)
        {
            //var myBrush = new ImageBrush { ImageSource = new BitmapImage(new Uri(VkApi.AudioResult()[0].UserAvatarUrl, UriKind.Absolute)) };
            //LogoutButton.Background = myBrush;
        }
        private void ListGroupsSmall_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var group = ListGroupsSmall.SelectedItem as DeserializedGropups;
            if (group == null) return;
            var myBrush = new ImageBrush { ImageSource = new BitmapImage(new Uri(group.photo_200, UriKind.Absolute)) };
            GroupAvatarEllipse.Fill = myBrush;
            GroupAvatarEllipse.Stroke = Brushes.Orange;
            GroupAvatarEllipse.StrokeThickness = 1;
            VkRequest.AudioGet("-"+group.id);
            ListMain.ItemsSource = VkRequest.AudioResult();
        }
    }
    }
    #endregion

