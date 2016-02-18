using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;

namespace Pizza
{
    public class Downloader:MainWindow
    {
        private static string _downloadedtrack;
        private WebClient _webclient;

        public void Download(string uri, string filename)
        {
            var localpath = Directory.GetCurrentDirectory();
            _webclient = new WebClient();
            _webclient.DownloadFileCompleted += WebclientOnDownloadFileCompleted;
            _webclient.DownloadProgressChanged += WebclientOnDownloadProgressChanged;
            if (uri == null) return;
            var dwnldtrackname = $@"{localpath}\{filename}.mp3";
            _webclient.DownloadFileAsync(new Uri(uri), dwnldtrackname);
            _downloadedtrack = filename + ".mp3";
        }

        private void WebclientOnDownloadProgressChanged(object sender, DownloadProgressChangedEventArgs downloadProgressChangedEventArgs)
        {
            //в разработке
        }

        private void WebclientOnDownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show($@"File {_downloadedtrack} is downloaded");
        }
        
    }
}