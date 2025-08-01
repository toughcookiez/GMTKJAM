using System.Collections;
using UnityEngine;

public class JumpSurface : MonoBehaviour
{

    BoxCollider2D bc;

    public bool isActive = false;
    public float timeUntilActive = 2;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bc = transform.parent.GetComponent<BoxCollider2D>();
        bc.isTrigger = true;

        StartCoroutine(BecomeActive());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive && other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().SuperJump();
        }
    }

    IEnumerator BecomeActive()
    {
        yield return new WaitForSeconds(timeUntilActive);

       isActive = true;
        bc.isTrigger = false;
    }
}
