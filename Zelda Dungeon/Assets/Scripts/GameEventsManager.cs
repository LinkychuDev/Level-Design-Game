using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance;

    public bool hasUnlockedFireAndIce;

    public GameObject mapObject;
    public event Action UnlockedFireAndIce;

    void Awake()
    {
        instance = this;
        mapObject.SetActive(false);
        DontDestroyOnLoad(this);
    }

    public void UnlockedMap()
    {
        mapObject.SetActive(true);
    }

    public void UnlockedFireAndIceEvent()
    {
        hasUnlockedFireAndIce = true;
        UnlockedFireAndIce?.Invoke();
    }
}