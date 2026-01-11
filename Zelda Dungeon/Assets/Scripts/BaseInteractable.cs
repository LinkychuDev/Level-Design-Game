using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseInteractable : MonoBehaviour
{
    public bool canInteract = true;

    public abstract void OnInteract();
}