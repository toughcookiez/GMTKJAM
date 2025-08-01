using UnityEngine;

public class DestroyRadius : MonoBehaviour
{

    private PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = transform.parent.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("SpawnObject") || other.gameObject.CompareTag("Block") || other.gameObject.CompareTag("Harmful"))
        {
            Destroy(other.gameObject);
            GameObject breakEffect = Instantiate(player.BlockBreakEffect, other.gameObject.transform.position, Quaternion.identity);
            breakEffect.GetComponent<ParticleSystem>().startColor = other.gameObject.GetComponent<BlockColor>()._breakColor;

            //Get points for destroyed objects
            player.points += player.pointsPerDestroyedObject;
        }
    }
}