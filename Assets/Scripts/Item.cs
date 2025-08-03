using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType Type;

    [SerializeField] public float coinAmount = 10;

    private PlayerController player;


    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }


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
                    collision.GetComponent<PlayerController>().points += coinAmount;
                    Destroy(gameObject);
                    break;
                case ItemType.Coin:

                    collision.GetComponent<PlayerController>().points += coinAmount;
                    Destroy(gameObject);
                    break;



            }
        } 
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Harmful") || collision.CompareTag("Block") || collision.CompareTag("SpawnObject"))
        {
            Destroy(collision.gameObject);
            GameObject breakEffect = Instantiate(player.BlockBreakEffect, collision.gameObject.transform.position, Quaternion.identity);
            breakEffect.GetComponent<ParticleSystem>().startColor = collision.gameObject.GetComponent<BlockColor>()._breakColor;
        }
    }

}

 public enum ItemType
{
    Coin,
    Heart
}
