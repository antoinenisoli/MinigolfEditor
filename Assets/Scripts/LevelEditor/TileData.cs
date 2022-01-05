using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileData
{
    public int x, y;
    public int tileIndex;
    public GameObject tilePrefab;
    public Mesh mesh;
    public float Yrotation;

    public TileData(int x, int y, int tileIndex, Mesh mesh, float Yrotation, GameObject tilePrefab)
    {
        this.x = x;
        this.y = y;
        this.tileIndex = tileIndex;
        this.mesh = mesh;
        this.Yrotation = Yrotation;
        this.tilePrefab = tilePrefab;
    }

    public override string ToString()
    {
        return "|" + x + ";" + y + ";" + tileIndex + ";" + mesh.name + ";" + Yrotation;
    }
}
