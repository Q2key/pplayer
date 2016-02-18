using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Threading;
using System.Web.UI.WebControls;
using System.Windows;
using ListView = System.Windows.Controls.ListView;

namespace Pizza
{
    public class SQliteQerry
    {
        private static SQLiteConnection _con;
        public static void CreateDbAsync()
        {
            CreateAudioDb(VkRequest.AudioResult());
        }
        private static void CreateAudioDb(List<AudioResult> vkaudiolist)
        {
            //if (CreateBaseAndOpenConnection() != 0) { return; } 
            CreateBaseAndOpenConnection();
            using (_con)
            {
                if (_con.State != ConnectionState.Open) return;
                CreateDbTable();
                InsertDataToDb(vkaudiolist);
                _con.Close();
            }
        }
        private static int CreateBaseAndOpenConnection()
        {
            var index = 0;
            const string dbname = "audiobase.sqlite";
            var path = Directory.GetCurrentDirectory();
            var curentdbpath = path + @"\Data\Db\" + dbname;
            var conncstring = "Data Source =" + Directory.GetCurrentDirectory() + @"\Data\Db\" + dbname + ";Version=3;";
            if (!File.Exists(curentdbpath))//Create .sqlite file is no exist
            {
                SQLiteConnection.CreateFile(curentdbpath);
                _con = new SQLiteConnection(conncstring);
                return index;
            }
            _con = new SQLiteConnection(conncstring);
            _con.Open();
            index = 1;
            return index;
        }
        private static void CreateDbTable()
        {
            var cmd = _con.CreateCommand();
            const string createaudiotablecmd = "DROP TABLE IF EXISTS audio_base; "
                                               + "DROP TABLE IF EXISTS user_base; "
                                               + "DROP TABLE IF EXISTS groups_audio_base; "
                                               + "CREATE TABLE audio_base("
                                               + "id INTEGER PRIMARY KEY AUTOINCREMENT, "
                                               + "ArtistAndTitle TEXT, "
                                               + "Title TEXT, "
                                               + "Duration TEXT, "
                                               + "UserAvatarUrl TEXT, "
                                               + "Url TEXT);"
                                               //create user data table
                                               + "CREATE TABLE user_base("
                                               + "id INTEGER PRIMARY KEY AUTOINCREMENT, "
                                               + "NameGen TEXT, "
                                               + "UserAvatarUrl TEXT);"
                                               //create group audio data table
                                               + "CREATE TABLE groups_audio_base("
                                               + "id INTEGER PRIMARY KEY AUTOINCREMENT, "
                                               + "ArtistAndTitle NameGen, "
                                               + "UserAvatarUrl TEXT);";
            cmd.CommandText = createaudiotablecmd;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SQLiteException ex)
            {
                MessageBox.Show(MessageBox.Show(ex.Message).ToString());
            }
        }
        private static void InsertDataToDb(List<AudioResult> vkaudiolist)
        {
            if (CreateBaseAndOpenConnection() == 0) return;
            using (var transaction = _con.BeginTransaction())
            {
                for (var i = 0; i < vkaudiolist.Count; i++)
                {
                    if (i == 0)
                    {
                        const string usqlCommand = "INSERT INTO user_base(NameGen, UserAvatarUrl) VALUES (@NameGen,@UserAvatarUrl);";
                        var uparamcmd = new SQLiteCommand(usqlCommand, _con) { CommandText = usqlCommand };
                        uparamcmd.Parameters.AddWithValue("@NameGen", vkaudiolist[i].UserNameGen);
                        uparamcmd.Parameters.AddWithValue("@UserAvatarUrl", vkaudiolist[i].UserAvatarUrl);
                        try
                        {
                            uparamcmd.ExecuteNonQuery();
                        }
                        catch (SQLiteException ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    const string sqlCommand =
                        "INSERT INTO audio_base(ArtistAndTitle, Title, Duration,UserAvatarUrl, Url) VALUES (@ArtistAndTitle,@Title,@Duration,@UserAvatarUrl,@Url);";
                    var paramcmd = new SQLiteCommand(sqlCommand, _con) { CommandText = sqlCommand };
                    paramcmd.Parameters.AddWithValue("@ArtistAndTitle", vkaudiolist[i].ArtistAndTitle);
                    paramcmd.Parameters.AddWithValue("@Title", vkaudiolist[i].Title);
                    paramcmd.Parameters.AddWithValue("@Duration", vkaudiolist[i].Duration);
                    paramcmd.Parameters.AddWithValue("@UserAvatarUrl", vkaudiolist[i].UserAvatarUrl);
                    paramcmd.Parameters.AddWithValue("@Url", vkaudiolist[i].Url);
                    try
                    {
                        paramcmd.ExecuteNonQuery();
                    }
                    catch (SQLiteException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                transaction.Commit();
            }
        }
        public static List<SqLiteDataObjects> SqliteDataExtract()
        {
            const string dbname = "audiobase.sqlite";
            var conncstring = "Data Source =" + Directory.GetCurrentDirectory() + @"\Data\Db\" + dbname + ";Version=3;";
            _con = new SQLiteConnection(conncstring);
            _con.Open();
            var sqlCmd = _con.CreateCommand();
            sqlCmd.CommandText = "SELECT * FROM audio_base; SELECT * FROM user_base";
            var datalist = new List<SqLiteDataObjects>();
            var da = new SQLiteDataAdapter(sqlCmd.CommandText, _con);
            da.TableMappings.Add("audio_base", "audio_base");
            da.TableMappings.Add("user_base", "user_base");
            da.TableMappings.Add("groups_audio_base", "groups_audio_base");
            var ds = new DataSet();
            da.Fill(ds);
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                datalist.Add(new SqLiteDataObjects
                {
                    ArtistAndTitle = row.ItemArray[1].ToString(),
                    Duration = row.ItemArray[3].ToString(),
                    UserAvatarUrl = row.ItemArray[4].ToString(),
                    Url = row.ItemArray[5].ToString()
                });
            }
            datalist.Add(new SqLiteDataObjects { UserAvatarUrl = ds.Tables[1].Rows[0].ItemArray[2].ToString() });
            return datalist;
        }
    }
}