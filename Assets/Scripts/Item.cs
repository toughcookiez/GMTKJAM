using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType Type;

    [SerializeField] private float coinAmount = 10;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            switch (Type)
            {
                case ItemType.Heart:
                    if (collision.GetComponent<PlayerController>().health == collision.GetComponent<PlayerController>().maxHealth)
                    {
                        Destroy(gameObject);
                        return;
                    }
                    collision.GetComponent<PlayerController>().health += 1;
                    Destroy(gameObject);
                    break;
                case ItemType.Coin:

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
