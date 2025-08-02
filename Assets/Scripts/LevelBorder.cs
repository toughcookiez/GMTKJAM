using UnityEngine;


//Anything that leaves the level border must be destroyed
public class LevelBorder : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerController>().PlayerDies();
        }
        else if(other.gameObject.CompareTag("Harmful")|| other.gameObject.CompareTag("Block"))
        {
            Destroy(other.gameObject);
        }
        
    }
}
