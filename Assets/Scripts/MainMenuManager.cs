using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject playPanel, mainPanel;

    private void Start()
    {
        MainMenuPanel();
    }

    public void PlayButton()
    {
        mainPanel.SetActive(false);
        playPanel.SetActive(true);
    }

    public void ChooseLevel(int i)
    {
        SceneManager.LoadScene(i);
    }

    public void MainMenuPanel()
    {
        mainPanel.SetActive(true);
        playPanel.SetActive(false);
    }

    public void EditorButton()
    {
        SceneManager.LoadScene("LevelEditor");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
