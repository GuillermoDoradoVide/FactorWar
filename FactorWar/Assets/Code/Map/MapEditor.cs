using UnityEngine;
using System.IO;

public class MapEditor : MonoBehaviour {

    public Map map;

    public void Save()
    {
        string path = Path.Combine(Application.persistentDataPath, "test.map");
        Stream fileStream = File.Open(path, FileMode.Create);
        fileStream.Close();
        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            writer.Write(0);
            map.Save(writer);
        }
    }

    public void Load()
    {
        string path = Path.Combine(Application.persistentDataPath, "test.map");
        using (BinaryReader reader = new BinaryReader(File.OpenRead(path)))
        {
            int header = reader.ReadInt32();
            if (header == 0)
            {
                map.Load(reader);
            }
            else
            {
                Debugger.printErrorLog("Unkown map format " + header);
            }
            
        }
    }

}
