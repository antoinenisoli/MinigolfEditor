using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum GameStatus
{
    None,
    Playing,
    Failed,
    Complete
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentLevelIndex;
    public GameStatus gameStatus = GameStatus.None;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void SetStatus(GameStatus status)
    {
        gameStatus = status;
        switch (gameStatus)
        {
            case GameStatus.Complete:
                SoundManager.instance.PlayFx(FxTypes.GAMECOMPLETEFX);
                break;
            case GameStatus.Failed:
                SoundManager.instance.PlayFx(FxTypes.GAMEOVERFX);
                break;
        }
    }
}