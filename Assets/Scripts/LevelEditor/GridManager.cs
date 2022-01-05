using System.Collections;
using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public TileData CurrentTileData;

    public GameObject tilePrefab;
    public Vector2Int gridSize;
    [SerializeField] Vector3Int gridPos;
    [SerializeField] Transform roadPreview;
    [SerializeField] Material freeMat, stuckMat;
    public GameObject objectToSpawn { get; set; }
    public bool placingPlayer, placingHole;
    public Vector3 playerPos, holePos;

    Vector3 currentRotation;
    Camera mainCam;
    public int[,] map;
    public Dictionary<Vector2Int, Tile> tilesDictionary = new Dictionary<Vector2Int, Tile>();

    private void Awake()
    {
        if (!Instance)
            Instance = this;

        objectToSpawn = tilePrefab;
        CurrentTileData.mesh = roadPreview.GetComponentInChildren<MeshFilter>().sharedMesh;
        map = new int[gridSize.x, gridSize.y];
        mainCam = Camera.main;
    }

    public Tile GetRoadTile(Vector2Int coordinates)
    {
        if (tilesDictionary.TryGetValue(coordinates, out Tile tile))
            return tile;

        return null;
    }

    public void ClearGrid()
    {
        map = new int[gridSize.x, gridSize.y];
        foreach (var item in tilesDictionary.Values)
        {
            Destroy(item.gameObject);
        }

        tilesDictionary.Clear();
    }

    public void PlaceTile(Vector3Int pos)
    {
        Vector2Int coordinates = new Vector2Int(pos.x, pos.z);
        if (!InRange(coordinates.x, coordinates.y))
            return;
        if (map[coordinates.x, coordinates.y] == 1)
            return;

        GameObject newTileObj;
        if (objectToSpawn == tilePrefab)
        {
            map[coordinates.x, coordinates.y] = 1;
            newTileObj = Instantiate(objectToSpawn, pos, Quaternion.identity);
        }
        else
        {
            newTileObj = Instantiate(objectToSpawn, pos, Quaternion.Euler(currentRotation));
            objectToSpawn = tilePrefab;
            print(objectToSpawn);
            if (placingHole)
                holePos = pos;
            if (placingPlayer)
                playerPos = pos;

            placingPlayer = false;
            placingHole = false;
        }

        Tile tile = newTileObj.GetComponent<Tile>();
        if (tile)
        {
            tile.Create(coordinates);
            CurrentTileData.x = coordinates.x;
            CurrentTileData.y = coordinates.y;
            CurrentTileData.Yrotation = currentRotation.y;

            tile.visual.transform.rotation = Quaternion.Euler(currentRotation);
            tile.SetData(CurrentTileData);
            tilesDictionary.Add(coordinates, tile);
            EventManager.Instance.onNewTile.Invoke();
        }
    }

    public void PlaceSavedTile(TileData tileData)
    {
        if (!InRange(tileData.x, tileData.y))
            return;
        if (map[tileData.x, tileData.y] == 1)
            return;

        map[tileData.x, tileData.y] = 1;
        Vector3 pos = new Vector3(tileData.x, 0, tileData.y);
        GameObject newTileObj = Instantiate(objectToSpawn, pos, Quaternion.identity);

        Tile tile = newTileObj.GetComponent<Tile>();
        if (tile)
        {
            tile.visual.transform.rotation = Quaternion.Euler(currentRotation);
            tile.SetData(tileData);
            tilesDictionary.Add(tile.coordinates, tile);
            EventManager.Instance.onNewTile.Invoke();
        }
    }

    public void RemoveRoad(Vector3Int pos)
    {
        Vector2Int coordinates = new Vector2Int(pos.x, pos.z);
        if (!InRange(coordinates.x, coordinates.y))
            return;
        if (map[coordinates.x, coordinates.y] == 0)
            return;

        if (tilesDictionary.TryGetValue(coordinates, out Tile tile))
        {
            tile.Death();
            map[coordinates.x, coordinates.y] = 0;
            tilesDictionary.Remove(coordinates);
        }
    }

    public bool InRange(int x, int y)
    {
        if (x > gridSize.x || x < 0 || y > gridSize.y || y < 0)
            return false;

        return true;
    }

    void ShowDebug(Vector2Int coordinates)
    {
        if (InRange(coordinates.x, coordinates.y))
        {
            MeshRenderer renderer = roadPreview.GetComponentInChildren<MeshRenderer>();
            var materials = renderer.sharedMaterials;
            for (int i = 0; i < materials.Length; i++)
            {
                if (map[coordinates.x, coordinates.y] == 1)
                    materials[i] = stuckMat;
                else if (map[coordinates.x, coordinates.y] == 0)
                    materials[i] = freeMat;
            }

            renderer.sharedMaterials = materials;
        }
    }

    public void LoadMap()
    {
        ClearGrid();

        string mapName = LevelEditorUiManager.instance.mapTextInput.text;
        string savedData = File.ReadAllText(Application.dataPath + "/Maps/" + mapName + ".txt");
        MapData savedMap = JsonUtility.FromJson<MapData>(savedData);
        print(mapName + " loaded !");
        foreach (var item in savedMap.allTileDatas)
        {
            PlaceSavedTile(item);
        }
    }

    public void SaveMap()
    {
        if (tilesDictionary.Count <= 0)
            return;

        List<TileData> allTileDatas = new List<TileData>();
        foreach (var item in tilesDictionary.Values)
            allTileDatas.Add(new TileData(item.coordinates.x, item.coordinates.y, item.tileData.tileIndex, item.tileData.mesh, item.tileData.Yrotation, item.tileData.tilePrefab));

        string mapName = LevelEditorUiManager.instance.mapTextInput.text;
        MapData save = new MapData(mapName, allTileDatas, playerPos, holePos);
        string json = JsonUtility.ToJson(save);
        File.WriteAllText(Application.dataPath + "/Maps/" + mapName + ".txt", json);
        print(mapName + " saved !");
    }

    public void SetTile(TileData data)
    {
        objectToSpawn = tilePrefab;
        CurrentTileData = data;
        roadPreview.GetComponentInChildren<MeshFilter>().sharedMesh = data.mesh; 
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        Ray screenRay = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(screenRay, out RaycastHit hit))
        {
            Vector3 mousePos = hit.point;
            gridPos = new Vector3Int(Mathf.RoundToInt(mousePos.x), 0, Mathf.RoundToInt(mousePos.z));
            roadPreview.position = Vector3.Lerp(roadPreview.position, gridPos, 50f * Time.deltaTime);
            Vector2Int coordinates = new Vector2Int(gridPos.x, gridPos.z);
            if (Input.GetMouseButton(0))
                PlaceTile(gridPos);
            else if (Input.GetMouseButton(1))
                RemoveRoad(gridPos);

            ShowDebug(coordinates);

            float scrollWheel = Input.GetAxisRaw("Mouse ScrollWheel");
            if (scrollWheel != 0)
            {
                currentRotation += Vector3.up * (90 * scrollWheel * 10);
                roadPreview.GetChild(0).DOKill();
                roadPreview.GetChild(0).DORotate(currentRotation, 0.2f);
            }
        }
    }
}
