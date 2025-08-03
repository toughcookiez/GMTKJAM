using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType Type;

    [SerializeField] public float coinAmount = 10;
    private AudioSource source;

    public AudioClip coin;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (Type)
            {
                case ItemType.Heart:
                    source.PlayOneShot(coin);
                    if (collision.GetComponent<PlayerController>().health == collision.GetComponent<PlayerController>().maxHealth)
                    {
                        Destroy(gameObject);
                        return;
                    }
                    collision.GetComponent<PlayerController>().health += 1;
                    collision.GetComponent<PlayerController>().points += coinAmount;
                    Destroy(gameObject);
                    break;
                case ItemType.Coin:
                    source.PlayOneShot(coin);
                    collision.GetComponent<PlayerController>().points += coinAmount;
                    Destroy(gameObject);
                    break;



            }
        }
    }

}

 public enum ItemType
{
    Coin,
    Heart
}
