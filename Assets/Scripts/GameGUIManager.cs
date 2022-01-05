using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameGUIManager : MonoBehaviour
{
    public static GameGUIManager instance;
    [SerializeField] Slider powerSlider;       
    [SerializeField] Text shotText;
    Text gameOverText;
    [SerializeField] GameObject gameOverPanel;  

    public Text ShotText { get { return shotText; } }  
    public Slider PowerSlider { get => powerSlider; }         

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        powerSlider.value = 0;
        gameOverPanel.GetComponent<CanvasGroup>().DOFade(0, 0);
        gameOverText = gameOverPanel.GetComponentInChildren<Text>();
        gameOverPanel.SetActive(false);
    }

    public void GameResult()
    {
        print(GameManager.instance.gameStatus);
        Time.timeScale = 0.2f;
        switch (GameManager.instance.gameStatus)
        {
            case GameStatus.Complete:
                gameOverPanel.SetActive(true);
                gameOverText.text = "You win !";
                gameOverPanel.GetComponent<CanvasGroup>().DOFade(1, 0.3f).SetUpdate(true).OnComplete(() =>
                {
                    StartCoroutine(DelayedRestart(LoadNextBtn));
                });

                break;

            case GameStatus.Failed:
                gameOverPanel.SetActive(true);
                gameOverText.text = "Game over !";
                gameOverPanel.GetComponent<CanvasGroup>().DOFade(1, 0.3f).SetUpdate(true).OnComplete(() =>
                {
                    StartCoroutine(DelayedRestart(NextRetryBtn));
                });

                break;
        }
    }

    IEnumerator DelayedRestart(TweenCallback callback)
    {
        yield return new WaitForSecondsRealtime(4f);
        callback();
    }

    public void HomeBtn()
    {
        GameManager.instance.SetStatus(GameStatus.None);
        SceneManager.LoadScene(0);
    }

    public void NextRetryBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            if (GameManager.instance.gameStatus == GameStatus.Failed)
                NextRetryBtn();
            else if (GameManager.instance.gameStatus == GameStatus.Complete)
                LoadNextBtn();
        }
    }
}
