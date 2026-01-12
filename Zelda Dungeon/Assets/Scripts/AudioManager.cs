using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip waterClip;
    public AudioClip chestClip;
    public AudioClip bubblesClip;
    public AudioClip doorUnlockClip;
    public AudioClip burnClip;
    public AudioClip freezeClip;
    public AudioClip igniteClip;
    public AudioClip windClip;

    public float waterVolume,
        chestVolume,
        burnVolume,
        freezeVolume,
        igniteVolume,
        windVolume,
        gearVolume,
        buttonVolume,
        bubbleVolume,
        doorVolume,
        pressureVolume,
        electricVolume;
    public AudioClip windUpClip;
    public AudioClip buttonClip;
    public AudioClip pressureClip;
    public AudioClip electricityClip;
    public static AudioManager instance {get; private set;}


    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }


    public void PlayAudioClip(AudioClip clip, float audioVolume)
    {
        audioSource.PlayOneShot(clip, audioVolume);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
