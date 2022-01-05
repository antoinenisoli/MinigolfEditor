using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private Slider powerSlider;       
    [SerializeField] private Text shotText;        
    [SerializeField] private GameObject mainMenu, gameMenu, gameOverPanel, retryBtn, nextBtn;  
    [SerializeField] private GameObject container, lvlBtnPrefab;    

    public Text ShotText { get { return shotText; } }  
    public Slider PowerSlider { get => powerSlider; }         

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        powerSlider.value = 0;                        
    }

    void Start()
    {
        /*if (GameManager.singleton.gameStatus == GameStatus.None)    
        {   
            CreateLevelButtons();                    
        }     
        else if 
            (GameManager.singleton.gameStatus == GameStatus.Failed ||
            GameManager.singleton.gameStatus == GameStatus.Complete)
        {
            mainMenu.SetActive(false);                                
            gameMenu.SetActive(true);                               
            LevelManager.instance.SpawnLevel(GameManager.singleton.currentLevelIndex); 
        }*/
    }

    /*void CreateLevelButtons()
    {
        for (int i = 0; i < LevelManager.instance.levelData.Length; i++)
        {
            GameObject buttonObj = Instantiate(lvlBtnPrefab, container.transform);   
            buttonObj.transform.GetChild(0).GetComponent<Text>().text = "" + (i + 1); 
            Button button = buttonObj.GetComponent<Button>();                          
            button.onClick.AddListener(() => OnClick(button));                       
        }
    }*/

    void OnClick(Button btn)
    {
        mainMenu.SetActive(false);                                                     
        gameMenu.SetActive(true);                                                   
        GameManager.singleton.currentLevelIndex = btn.transform.GetSiblingIndex();  
        //LevelManager.instance.SpawnLevel(GameManager.singleton.currentLevelIndex);      
    }

    public void GameResult()
    {
        switch (GameManager.singleton.gameStatus)
        {
            case GameStatus.Complete:
                print("win!");
                gameOverPanel.SetActive(true);             
                nextBtn.SetActive(true);                   
                SoundManager.instance.PlayFx(FxTypes.GAMECOMPLETEFX);
                break;
            case GameStatus.Failed:
                print("lose!");
                gameOverPanel.SetActive(true);           
                retryBtn.SetActive(true);                 
                SoundManager.instance.PlayFx(FxTypes.GAMEOVERFX);
                break;
        }
    }

    public void HomeBtn()
    {
        GameManager.singleton.gameStatus = GameStatus.None;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextRetryBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
