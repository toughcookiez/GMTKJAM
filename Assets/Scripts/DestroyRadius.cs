using UnityEngine;

public class DestroyRadius : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        }
    }
}
