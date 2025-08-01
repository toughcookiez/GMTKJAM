using TMPro;
using UnityEngine;

public class PlayerText : MonoBehaviour
{

    PlayerController player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        
    }

    // Update is called once per frame
    void Update()
    {
        
        GetComponent<TextMeshProUGUI>().text = "Health: " + player.health + " Lives: " + player.lives + " Points: " + player.points;
    }
}
