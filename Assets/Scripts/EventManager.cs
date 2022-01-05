using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;
    public UnityEvent onNewTile = new UnityEvent();
    public class TileEvent : UnityEvent<TileData> { }

    public TileEvent onButtonSelection = new TileEvent();

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }
}
