using UnityEngine;


//Script for Objects that may be affected by JumpPad(Player and (anti)gravity blocks)
public class Jumpable : MonoBehaviour
{


    Rigidbody2D rb;
    public float jumpForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);


    }
}
