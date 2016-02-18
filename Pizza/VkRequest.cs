using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI.WebControls;
using System.Windows;
using Awesomium.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebControl = Awesomium.Windows.Controls.WebControl;

namespace Pizza
{
    public delegate void RefreshUiEventHandler();
    public delegate void RequestErrorEventHandler();
    public class VkRequest:MainWindow
    {         
        public static event RefreshUiEventHandler RefreshUi;
        public static event RequestErrorEventHandler ReqquestError;
        private static List<DeserializedSerachResult> _dsersearchresult;
        private static List<DeserializedVkAudio> _deserializedAuioList;
        private static List<DeserializedGropups> _deserializedGropupList; 
        private static List<AudioResult> _audioresults;
        private static List<SearchResult> _searchResults;       
        private static WebControl _aweb;
        private static WebSession _websession;
        public static void CreateBro(MainWindow mw)
        {         
                _aweb = new WebControl();
                mw.BroGrid.Children.Add(_aweb);
                _aweb.Visibility = Visibility.Visible;
                if (_websession == null)
                {
                    _websession = WebCore.CreateWebSession(WebPreferences.Default);
                    _aweb.WebSession = _websession;
                }
                _aweb.LoadingFrameComplete += BrowserOnLoadingFrameComplete;
                _aweb.Source = new Uri("https://oauth.vk.com/authorize?client_id=5086924&display=popup&" +
                    "redirect_uri=https://oauth.vk.com/blank.html&scope=audio,offline&response_type=token&v=5.44");
        }
        private static void BrowserOnLoadingFrameComplete(object sender, FrameEventArgs frameEventArgs)
        {
            var url = _aweb.Source.AbsoluteUri;
            if (_aweb.Source.AbsolutePath != "/blank.html") return;
            var furl = url.Split('#')[1];
            if (furl[0] == 'a')
            {
                AppSettings.Default.acesstoken = furl.Split('&')[0].Split('=')[1];
                AppSettings.Default.id = furl.Split('=')[3];
                AppSettings.Default.aut = true;
                Keys.Serialize(AppSettings.Default.acesstoken, AppSettings.Default.id);
            }
            _aweb.Visibility = Visibility.Hidden;
            _aweb.Dispose();
            CreateRequestAndRefreshUi();
        }
        public static void CreateRequestAndRefreshUi()//Запуск функции запросов (ApiRequests) в фоновом потоке.
        {
            var thread = new Thread(ApiRequests)
            {
                IsBackground = true,
                Name = "BrowserRequestThread"
            };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
        }
        public static void ApiRequests()
        {
            if (Keys.IsKeyEmpty()) return;
            AudioGet(AppSettings.Default.id);
            GroupsGet();
            RefreshUi?.Invoke();
        }
        public static void Logout(MainWindow mw)
        {
            Keys.DeleteKeys();
            AppSettings.Default.aut = false;
            if (_websession == null) return;
            _websession.ClearCookies();
            _websession.ClearCache();
        }
        private static string CheckReqError (JObject jObject)
        {
            if (jObject.ToString().Contains("response"))
            {
                return string.Empty;
            }
            var result = jObject["error"]["error_code"].ToString();
            return result;
        }
        public static void Search(string querry)
        {           
            var webRequest =
                WebRequest.Create(
                    "https://api.vk.com/method/audio.search?q="+querry+"&auto_complete=1&sort=2&v=5.44&access_token="+AppSettings.Default.acesstoken);
            var vkaudiorResponse = webRequest.GetResponse();
            var datastream = vkaudiorResponse.GetResponseStream();
            if (datastream == null) return;
            var reader = new StreamReader(datastream);
            var responsefromserver = reader.ReadToEnd();
            reader.Close();
            vkaudiorResponse.Close();
            responsefromserver = HttpUtility.HtmlDecode(responsefromserver);
            var jtoken = JToken.Parse(responsefromserver);
            var jObject = JObject.Parse(jtoken.ToString());
            if (CheckReqError(jObject).Contains("5"))
            {
                ReqquestError?.Invoke();
                return;
            }
            _dsersearchresult = JsonConvert.DeserializeObject<List<DeserializedSerachResult>>(jObject["response"]["items"].ToString());
        }
        public static void GroupSearch(string querry)
        {
            var webRequest =
                WebRequest.Create(
                    "https://api.vk.com/method/audio.search?q=" + querry + "&auto_complete=1&sort=2&v=5.44&access_token=" + AppSettings.Default.acesstoken);
            var vkaudiorResponse = webRequest.GetResponse();
            var datastream = vkaudiorResponse.GetResponseStream();
            if (datastream == null) return;
            var reader = new StreamReader(datastream);
            var responsefromserver = reader.ReadToEnd();
            reader.Close();
            vkaudiorResponse.Close();
            responsefromserver = HttpUtility.HtmlDecode(responsefromserver);
            var jtoken = JToken.Parse(responsefromserver);
            var jObject = JObject.Parse(jtoken.ToString());
            if (CheckReqError(jObject).Contains("5"))
            {
                return;
            }
            _dsersearchresult = JsonConvert.DeserializeObject<List<DeserializedSerachResult>>(jObject["response"]["items"].ToString());
        }
        public static void AudioGet(string requestid)//Десериализация + Запрос VkApi.
        {
            var webRequest =
                WebRequest.Create(
                    "https://api.vk.com/method/audio.get?owner_id=" +
                    requestid + "&need_user=1&v=5.44&access_token=" +
                    AppSettings.Default.acesstoken);           
            var vkaudiorResponse = webRequest.GetResponse();
            var datastream = vkaudiorResponse.GetResponseStream();
            if (datastream == null) return;
            var reader = new StreamReader(datastream);
            var responsefromserver = reader.ReadToEnd();
            reader.Close();
            vkaudiorResponse.Close();
            responsefromserver = HttpUtility.HtmlDecode(responsefromserver);
            var jtoken = JToken.Parse(responsefromserver);
            var jObject = JObject.Parse(jtoken.ToString());
            var path = @"D:\json.txt";//для логирования ответа.
            File.WriteAllText(path, jtoken.ToString(), Encoding.Default);
            if (CheckReqError(jObject).Contains("5"))//Проверка Json ответа на код ошибки (5 - невалидный токен)
            {
                ReqquestError?.Invoke();//Событие вызывает функцию принудительного релогина MainWindow.ForceRelogin. (Аналогично во всех запросоах)
                return;
            }
            _deserializedAuioList = JsonConvert.DeserializeObject<List<DeserializedVkAudio>>(jObject["response"]["items"].ToString());
            CreateVkAudioList();
        }
        private static void CreateVkAudioList()//Функция создает объекты класса AudioResult. 
        {
            _audioresults = new List<AudioResult>();
            try
            {
                for (var index = 1; index < _deserializedAuioList.Count; index++)
                {
                    var serializedvktrack = _deserializedAuioList[index];
                    var uavatar = _deserializedAuioList[0].photo;
                    var s = serializedvktrack.duration;
                    var h = s / 3600;
                    var m = (s - (h * 3600)) / 60;
                    s = s - (h * 3600 + m * 60);
                    _audioresults.Add(new AudioResult
                    {
                        ArtistAndTitle = serializedvktrack.artist + " - " + serializedvktrack.title,
                        Artist = serializedvktrack.artist,
                        Title = serializedvktrack.title,
                        Duration = $@"{m:D}:{s:D2}",
                        Url = serializedvktrack.url,
                        UserNameGen = serializedvktrack.name_gen,
                        UserAvatarUrl = uavatar
                    });
                }
            }
            catch (NullReferenceException e)
            {
                MessageBox.Show(e.Message);
            }
        }
        public static List<AudioResult> AudioResult()//Возарвщает список объектов <AudioResul> для загрузки в UI
        {
            return _audioresults;
        }
        private static void GroupsGet()
        {
            var webRequest =
                WebRequest.Create(
                    "https://api.vk.com/method/groups.get?user_id=" +
                    AppSettings.Default.id + "&extended=1&v=5.44&access_token=" +
                    AppSettings.Default.acesstoken);
            var vkaudiorResponse = webRequest.GetResponse();
            var datastream = vkaudiorResponse.GetResponseStream();
            if (datastream == null) return;
            var reader = new StreamReader(datastream);
            var responsefromserver = reader.ReadToEnd();
            reader.Close();
            vkaudiorResponse.Close();
            responsefromserver = HttpUtility.HtmlDecode(responsefromserver);
            var jtoken = JToken.Parse(responsefromserver);
            var jObject = JObject.Parse(jtoken.ToString());
            if (CheckReqError(jObject).Contains("5"))
            {
                return;
            }
            _deserializedGropupList = JsonConvert.DeserializeObject<List<DeserializedGropups>>(jObject["response"]["items"].ToString());
        }
        public static List<DeserializedGropups> GroupsResult()
        {
            return _deserializedGropupList;
        }
        public static List<SearchResult> SearchResults()
        {
            _searchResults = new List<SearchResult>();
            if (_dsersearchresult == null) return _searchResults;
            foreach (var result in _dsersearchresult)
            {
                var s = result.duration;
                var h = s / 3600;
                var m = (s - (h * 3600)) / 60;
                s = s - (h * 3600 + m * 60);
                _searchResults.Add(new SearchResult
                {
                    ArtistAndTitle = result.artist + " - " + result.title,
                    Artist = result.artist,
                    Title = result.title,
                    Duration = $@"{m:D}:{s:D2}",
                    Url = result.url,
                });
            }
            return _searchResults;
        }
    }
}