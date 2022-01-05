using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public GameObject ballPrefab;        
    public LevelData levelData;       
    private int shotCount = 0;            

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Start()
    {
        shotCount = levelData.shotCount;                                  
        UIManager.instance.ShotText.text = shotCount.ToString();                     
        GameManager.singleton.gameStatus = GameStatus.Playing;     
    }

    public void NewShot()
    {
        if (shotCount > 0)                                        
        {
            shotCount--;                                           
            UIManager.instance.ShotText.text = "" + shotCount;     

            if (shotCount <= 0)                                   
                LevelFailed();                                       
        }
    }

    public void LevelFailed()
    {
        if (GameManager.singleton.gameStatus == GameStatus.Playing) 
        {
            GameManager.singleton.gameStatus = GameStatus.Failed;   
            UIManager.instance.GameResult();                       
        }
    }

    public void LevelComplete()
    {
        if (GameManager.singleton.gameStatus == GameStatus.Playing) 
        {   
            /*if (GameManager.singleton.currentLevelIndex < levelData.Length)    
                GameManager.singleton.currentLevelIndex++;  
            else
                GameManager.singleton.currentLevelIndex = 0;*/

            GameManager.singleton.gameStatus = GameStatus.Complete; 
            UIManager.instance.GameResult();                        
        }
    }
}
