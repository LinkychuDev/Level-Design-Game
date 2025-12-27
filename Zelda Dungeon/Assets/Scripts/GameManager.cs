using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}
    private Vector3 playerStartPosition;
    private Transform player;

    [SerializeField] private Vector2 outOfBoundsMin;
    [SerializeField] private Vector2 outOfBoundsMax;
    private void Awake()
    {
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStartPosition = player.position;
    }

    public void Death()
    {
        player.position = playerStartPosition;
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfOoB();
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
