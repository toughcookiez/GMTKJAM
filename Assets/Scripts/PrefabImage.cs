using UnityEngine;
using UnityEngine.UI;

public class PrefabImage : MonoBehaviour
{


    private PlayerController player;

    private Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        UpdateImage();
    }

    // Update is called once per frame
    public void UpdateImage()
    {
        if (player == null)
        {
            return;
        }
        if (player.nextPrefab == null)
        {
            return;
        }
        image.sprite = player.nextPrefab.GetComponent<SpriteRenderer>().sprite;
    }
}
