using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;

    public List<MapData> savedMaps = new List<MapData>();
    Dictionary<string, MapData> mapsDictionary = new Dictionary<string, MapData>();

    private void Awake()
    {
        if (!instance)
            instance = this;

        DontDestroyOnLoad(gameObject);
        GetMaps();
    }

    public MapData GetMap(string name)
    {
        return mapsDictionary[name];
    }

    public void GetMaps()
    {
        string[] files = Directory.GetFiles(Application.dataPath + "/Maps/");
        foreach (string file in files)
        {
            if (file.Contains(".meta"))
                continue;

            print(file);
            string savedData = File.ReadAllText(file);
            MapData map = JsonUtility.FromJson<MapData>(savedData);
            if (map != null)
            {
                savedMaps.Add(map);
                mapsDictionary.Add(map.mapName, map);
            }
        }
    }
}
