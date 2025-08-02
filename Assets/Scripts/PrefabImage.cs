using UnityEngine;
using UnityEngine.UI;

public class PrefabImage : MonoBehaviour
{

    [SerializeField]
    private PrefabType prefabType;

    private PlayerController player;

    private Image image;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        image = GetComponent<Image>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        UpdateImage();
    }

    public void UpdateImage()
    {
        if (prefabType == PrefabType.NextNext)
        {
            UpdateNextNextImage();
        }
        else
        {
            UpdateNextImage();
        }
        
    }

    // Update is called once per frame
    public void UpdateNextImage()
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

    // Update is called once per frame
    public void UpdateNextNextImage()
    {
        if (player == null)
        {
            return;
        }
        if (player.nextNextPrefab == null)
        {
            return;
        }
        image.sprite = player.nextNextPrefab.GetComponent<SpriteRenderer>().sprite;
    }
}

public enum PrefabType
{
    Next = 1,
    NextNext = 2
}
