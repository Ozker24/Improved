using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class PlayerSaveSystem 
{
    public static void SavePlayer(PlayerController player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Player.IMPR";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerSave data = new PlayerSave(player);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerSave LoadPlayer()
    {
        string path = Application.persistentDataPath + "/Player.IMPR";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerSave data = formatter.Deserialize(stream) as PlayerSave;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("There is no file in path: " + path);
            return null;
        }
    }
}
