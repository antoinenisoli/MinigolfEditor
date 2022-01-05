using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
    public string mapName;
    public List<TileData> allTileDatas = new List<TileData>();
    public Vector3 playerPosition;
    public Vector3 holePosition;

    public MapData(string mapName, List<TileData> allTileDatas, Vector3 playerPosition, Vector3 holePosition)
    {
        this.mapName = mapName;
        this.allTileDatas = allTileDatas;
        this.playerPosition = playerPosition;
        this.holePosition = holePosition;
    }
}
