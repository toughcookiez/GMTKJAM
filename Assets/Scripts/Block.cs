using System.Collections;
using UnityEngine;

public class Block : MonoBehaviour
{


    public float timeUntilActive = 0.1f;

    private BoxCollider2D bc;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bc = GetComponent<BoxCollider2D>();
        bc.isTrigger = true;

        StartCoroutine(BecomeActive());
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //Makes the block active (box collider is not trigger anymore) after waiting
    IEnumerator BecomeActive()
    {
        yield return new WaitForSeconds(timeUntilActive);

        bc.isTrigger = false;
    }
}
