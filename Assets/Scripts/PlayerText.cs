using TMPro;
using UnityEngine;

public class PlayerText : MonoBehaviour
{

    [SerializeField] private TextType type;
    
    [SerializeField] private PlayerController player;


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
