
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml;
using VkNet;
using Iceberg;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using testWPF;
using VkNet.Enums.Filters;
using VkNet.Model.Attachments;
using File = TagLib.File;
using MessageBox = System.Windows.MessageBox;
using Newtonsoft.Json;

namespace Iceberg
{
    [Serializable]
    public partial class Tracks

    {   
               
        public string TrackArtist { get; set; }
        public string TrackTitle { get; set; }
        public string TrackPath { get; set; }
        public string TrackBitrate { get; set; }
        public string TrackDuration { get; set; }

    }
}