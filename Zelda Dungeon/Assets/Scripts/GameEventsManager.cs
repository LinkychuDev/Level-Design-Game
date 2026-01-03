using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance;

    public bool hasUnlockedFireAndIce;

    public event Action UnlockedFireAndIce;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
    }


    public void UnlockedFireAndIceEvent()
    {
        hasUnlockedFireAndIce = true;
        UnlockedFireAndIce?.Invoke();
    }
}