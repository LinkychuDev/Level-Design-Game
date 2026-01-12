using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}
    private Vector3 playerStartPosition;
    private Transform player;


    public Canvas KeyCanvas;
    public TextMeshProUGUI KeyText;
    public bool hasGottenFirstKey;
    public int keyCount;
    [SerializeField] private Vector2 outOfBoundsMin;
    [SerializeField] private Vector2 outOfBoundsMax;
    
    private void Awake()
    {
        instance = this;
    }

    public bool UseKey()
    {
        if (keyCount > 0)
        {
            keyCount--;
            return true;
        }
        
        return false;
       
    }


    public void ObtainKey()
    {
        if (!hasGottenFirstKey)
        {
            KeyCanvas.gameObject.SetActive(true);
            hasGottenFirstKey = true;
        }
        
        keyCount++;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStartPosition = player.position;
        PlayerState.instance.UpdateLastGroundedPosition(playerStartPosition);
    }

    public void Checkpoint()
    {
        playerStartPosition = PlayerState.instance.lastGroundedPosition;
    }
    public void Death()
    {
        player.position = playerStartPosition;
      
    }

    // Update is called once per frame
    void Update()
    {
        UpdateKeyCountText();
        CheckIfOoB();
    }

    void UpdateKeyCountText()
    {
        KeyText.text = keyCount.ToString();
    }
    private void CheckIfOoB()
    {
        if (player.transform.position.x < outOfBoundsMin.x)
        {
            Death();
        }
        if (player.transform.position.x > outOfBoundsMax.x)
        {
            Death();
        }

        if (player.transform.position.y < outOfBoundsMin.y)
        {
            Death();
        }

        if (player.transform.position.y > outOfBoundsMax.y)
        {
            Death();
        }
    }
}
