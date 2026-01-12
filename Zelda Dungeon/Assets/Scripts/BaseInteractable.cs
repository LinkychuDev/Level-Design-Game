using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public abstract class BaseInteractable : MonoBehaviour
{
    internal AudioSource audioSource;
    public bool canInteract = true;

    public abstract void OnInteract();
}