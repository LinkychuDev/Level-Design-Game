using UnityEngine;

public class MiniMapScript : MonoBehaviour
{
    private Vector3 offset;

    private Transform player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        offset = transform.position - player.position;
    }
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.position + offset;
    }
}
