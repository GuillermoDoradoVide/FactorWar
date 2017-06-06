using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;

public class SaveLoadMap : MonoBehaviour {

    public Map map;
    public bool saveMode;
    public Text menuLabel, actionButtonLabel;
    public InputField inputField;
    public RectTransform listContent;
    public SaveLoadItem itemPrefab;

    public void Open(bool saveState)
    {
        saveMode = saveState;
        FillList();
        gameObject.SetActive(true);
        if(saveState)
        {
            menuLabel.text = " Save menu";
            actionButtonLabel.text = "SAVE";
        }
        else
        {
            menuLabel.text = " Load menu";
            actionButtonLabel.text = "LOAD";
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Save(string path)
    {
        Stream fileStream = File.Open(path, FileMode.Create);
        fileStream.Close();
        using (BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Create)))
        {
            writer.Write(0);
            map.Save(writer);
        }
    }

    public void Load(string path)
    {
        Debugger.printLog(path);
        if (!File.Exists(path))
        {
            Debug.LogError("File does not exist " + path);
            return;
        }
        
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

    string GetSelectedPath()
    {
        string mapName = inputField.text;
        if (mapName.Length == 0)
        {
            return null;
        }
        return Path.Combine(Application.persistentDataPath, mapName + ".map");
    }

    public void Action()
    {
        string path = GetSelectedPath();
        if (path == null)
        {
            return;
        }
        if (saveMode)
        {
            Save(path);
        }
        else
        {
            Load(path);
        }
        Close();
    }

    public void SelectItem(string name)
    {
        inputField.text = name;
    }

    void FillList()
    {
        for (int i = 0; i < listContent.childCount; i++)
        {
            Destroy(listContent.GetChild(i).gameObject);
        }
        string[] paths =
            Directory.GetFiles(Application.persistentDataPath, "*.map");
        Array.Sort(paths);
        for (int i = 0; i < paths.Length; i++)
        {
            SaveLoadItem item = Instantiate(itemPrefab);
            item.menu = this;
            item.MapName = Path.GetFileNameWithoutExtension(paths[i]);
            item.transform.SetParent(listContent, false);
        }
    }

    public void Delete()
    {
        string path = GetSelectedPath();
        if (path == null)
        {
            return;
        }
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        inputField.text = "";
        FillList();
    }

}
