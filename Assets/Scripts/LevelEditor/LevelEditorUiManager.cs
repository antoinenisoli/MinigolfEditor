using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelEditorUiManager : MonoBehaviour
{
    public static LevelEditorUiManager instance;
    [SerializeField] GameObject selectionButton;
    [SerializeField] Transform content;
    [SerializeField] GameObject[] tilePrefabs;
    public InputField mapTextInput;

    private void Awake()
    {
        if (!instance)
            instance = this;

        tilePrefabs.Reverse();
        for (int i = 0; i < tilePrefabs.Length; i++)
        {
            GameObject g = Instantiate(selectionButton, content);
            TileButton button = g.GetComponent<TileButton>();
            button.roadPrefab = tilePrefabs[i];
            button.SetData(new TileData(0, 0, i, tilePrefabs[i].GetComponent<MeshFilter>().sharedMesh, default, tilePrefabs[i]));
            button.Init();
        }
    }

    public void PlacePlayerSpawn(GameObject prefab)
    {
        GridManager.Instance.objectToSpawn = prefab;
        GridManager.Instance.placingPlayer = true;
    }

    public void PlaceHoleSpawn(GameObject prefab)
    {
        GridManager.Instance.objectToSpawn = prefab;
        GridManager.Instance.placingHole = true;
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
