using System.Collections;
using UnityEngine;

public class Spike : MonoBehaviour
{

    public float timeUntilHarmful = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.tag = null;
        StartCoroutine(BecomeHarmful());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator BecomeHarmful()
    {
        yield return new WaitForSeconds(timeUntilHarmful);

        gameObject.tag = "Harmful";
    }
}
