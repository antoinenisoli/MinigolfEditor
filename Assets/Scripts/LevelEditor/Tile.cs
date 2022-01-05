using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField]public Transform visual;
    public Vector2Int coordinates;
    MeshFilter meshFilter;
    public TileData tileData;

    private void Awake()
    {
        meshFilter = GetComponentInChildren<MeshFilter>();
    }

    public void Create(Vector2Int coordinates)
    {
        this.coordinates = coordinates;
        visual.localScale = Vector3.one * 0.01f;
        visual.DOScale(Vector3.one, 0.3f);
    }

    public void SetData(TileData data)
    {
        this.coordinates = new Vector2Int(data.x, data.y);
        tileData = data;
        tileData.x = coordinates.x;
        tileData.y = coordinates.y;
        meshFilter.mesh = data.mesh;
    }

    public void Death()
    {
        visual.DOScale(Vector3.one * 0.01f, 0.3f).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
