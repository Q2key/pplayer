using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Pizza
{
    [Serializable]
    public class Keys//Класс для управления ключами пользователя
    {
        public string Token { get; set; }
        public string Id { get; set; }
        public Keys(string token, string id)
        {
            Token = token;
            Id = id;
        }
        public static void Serialize(string token, string id)
        {
            var key = new Keys(token, id);
            var fs = new FileStream("k.dat", FileMode.OpenOrCreate);
            var formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, key);
            }
            catch (SerializationException e)
            {
                Console.WriteLine(@"Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }
        public static Keys Deserialize()
        {
            Keys key;
            var fs = new FileStream("k.dat", FileMode.OpenOrCreate);
            try
            {
                var formatter = new BinaryFormatter();
                key = (Keys)formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Console.WriteLine(@"Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
            return key;
        }
        public static bool IsKeyEmpty()//Проверка значений ключей пользователя.
        {
            if (AppSettings.Default.acesstoken == null || AppSettings.Default.id == null) return true;
            return false;
        }
        public static void DeleteKeys()
        {
            Serialize(null, null);
        }
        public static void RestoreKeys()
        {
            Deserialize();
            AppSettings.Default.acesstoken = Keys.Deserialize().Token;
            AppSettings.Default.id = Keys.Deserialize().Id;
        }
    }
}