using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class OptionSaveSystem 
{
    public static void SaveOptions(OptionsMenu OMenu)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Options.IMPR";
        FileStream stream = new FileStream(path, FileMode.Create);

        OptionsManager data = new OptionsManager(OMenu);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static OptionsManager LoadOptions()
    {
        string path = Application.persistentDataPath + "/Options.IMPR";

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            OptionsManager data = formatter.Deserialize(stream) as OptionsManager;
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
