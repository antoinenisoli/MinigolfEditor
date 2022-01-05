using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public int shotCount;

    [SerializeField] Transform levelFolder;
    [SerializeField] string mapName;
    [SerializeField] GameObject[] prefabs;
    MapData mapData;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Start()
    {
        mapData = MapManager.instance.GetMap(mapName);
        BuildLevel();
        GameGUIManager.instance.ShotText.text = shotCount.ToString();
        GameManager.instance.SetStatus(GameStatus.Playing);    
    }

    public void BuildLevel()
    {
        foreach (var item in mapData.allTileDatas)
        {
            Vector3 pos = new Vector3(item.x, 0, item.y);
            GameObject newTileObj = Instantiate(prefabs[item.tileIndex], pos, Quaternion.Euler(0, item.Yrotation, 0), levelFolder);
            newTileObj.AddComponent<MeshCollider>();
        }
    }

    public void NewShot()
    {
        if (shotCount > 0)                                        
        {
            shotCount--;                                           
            GameGUIManager.instance.ShotText.text = "" + shotCount;     

            if (shotCount <= 0)                                   
                LevelFailed();                                       
        }
    }

    public void LevelFailed()
    {
        if (GameManager.instance.gameStatus == GameStatus.Playing) 
        {
            GameManager.instance.SetStatus(GameStatus.Failed);
            GameGUIManager.instance.GameResult();                       
        }
    }

    public void LevelComplete()
    {
        if (GameManager.instance.gameStatus == GameStatus.Playing) 
        {
            GameManager.instance.SetStatus(GameStatus.Complete);
            GameGUIManager.instance.GameResult();                        
        }
    }
}
