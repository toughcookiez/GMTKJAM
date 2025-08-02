using TMPro;
using UnityEngine;

public class PlayerText : MonoBehaviour
{

    [SerializeField] private TextType type;
    PlayerController player;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        
    }

    // Update is called once per frame
    void Update()
    {
        switch (type)
        {
            case TextType.Lives:
                GetComponent<TextMeshProUGUI>().text = " Lives: " + player.lives;
                break;
            case TextType.Points:
                GetComponent<TextMeshProUGUI>().text = " Points: " + player.points;
                break;
        }

        
    }
}

public enum TextType
{
    Lives,
    Points
}
