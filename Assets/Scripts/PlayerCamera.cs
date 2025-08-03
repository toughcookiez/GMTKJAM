using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    PlayerController player;

    float offset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();


        offset = transform.position.x - player.transform.position.x;
    }


    //always set x position to player
    // Update is called once per frame
    void Update()
    {
        transform.position =  new Vector2(player.transform.position.x+offset, transform.position.y);
    }
}
